using BlazorApp2.Domain;
using BlazorApp2.Interfaces;

namespace BlazorApp2.Services;
/// <summary>
/// Hanterar skappande, hämtning och updatering av bankkonton
/// Synkroniserar data med lagringstjänsten och loggar transaktioner
/// </summary>
public class AccountService : IAccountService
{
    private const string StorageKey = "bank-accounts";
    private readonly IStorageService _storageService;
    private readonly ITransactionService _transactionService;
    private readonly List<BankAccount> _accounts = new();
    private bool _isLoaded;

    public AccountService(IStorageService storageService, ITransactionService transactionService)
    {
        _storageService = storageService;
        _transactionService = transactionService;
    }

    /// <summary>
    /// Säkerställer att kontodata är inläst från lagringen
    /// Körs automatiskt innan varje operation
    /// </summary>
    private async Task EnsureInitializedAsync()
    {
        if (_isLoaded) return;

        var fromStorage = await _storageService.LoadAsync<List<BankAccount>>(StorageKey);
        if (fromStorage is { Count: > 0 })
        {
            _accounts.AddRange(fromStorage);
            Console.WriteLine($"[AccountService] laddade {_accounts.Count} konton från LocalStorage");
        }

        _isLoaded = true;
    }

    /// <summary>
    /// Sparar den aktuella listan med konton till localstorage
    /// </summary>
    private async Task SaveAsync()
    {
        await _storageService.SaveAsync(StorageKey, _accounts);
    }

    /// <summary>
    /// Skapar ett nytt konto med angivna värden
    /// Loggar en starttransaktion om saldot är större än 0
    /// </summary>
    /// <param name="name"></param>
    /// <param name="accountType"></param>
    /// <param name="currency"></param>
    /// <param name="initialBalance"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<IBankAccount> CreateBankAccount(string name, AccountType accountType, string currency, decimal initialBalance)
    {
        await EnsureInitializedAsync();

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Kontonamn får inte vara tomt");
        }

        if (initialBalance < 0)
        {
            throw new ArgumentException("Start saldo kan inte vara negativt");
        }

        var account = new BankAccount(name, accountType, initialBalance, currency,DateTime.Now);
        _accounts.Add(account);
        await SaveAsync();

        if (initialBalance > 0)
        {
            await _transactionService.AddTransactionAsync(
                new Transaction(account.Id,TransactionType.Deposit, initialBalance,account.Balance,"Startsaldo"));
        }
        
        return account;
    }

    /// <summary>
    /// Hämtar alla konton som användaren har
    /// </summary>
    /// <returns></returns>
    public async Task<List<IBankAccount>> GetAccounts()
    {
        await EnsureInitializedAsync();
        return _accounts.Cast<IBankAccount>().ToList();
    }

    /// <summary>
    /// Hämtar ett enskilt konto baserat på dess ID
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    public async Task<IBankAccount?> GetAccount(Guid accountId)
    {
        await EnsureInitializedAsync();
        return _accounts.FirstOrDefault(a => a.Id == accountId);
    }

    /// <summary>
    /// Gör en insättning på ett konto
    /// Skapar en transaktion i historiken
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="amount"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task DepositAsync(Guid accountId, decimal amount)
    {
        await EnsureInitializedAsync();
        var account = _accounts.FirstOrDefault(a => a.Id == accountId)
                      ?? throw new InvalidOperationException("Konto hittades inte");
        
        account.Deposit(amount);
        await SaveAsync();
        
        await _transactionService.AddTransactionAsync(
            new Transaction(account.Id,TransactionType.Deposit, amount,account.Balance,"Insättning"));
    }

    /// <summary>
    /// Gör ett uttag från ett konto
    /// skapar en transaktion i historiken
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="amount"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task WithdrawAsync(Guid accountId, decimal amount)
    {
        await EnsureInitializedAsync();
        var account = _accounts.FirstOrDefault(a => a.Id == accountId)
            ?? throw new InvalidOperationException("Konto hittades inte");
        
        account.Withdraw(amount);
        await SaveAsync();

        await _transactionService.AddTransactionAsync(
            new Transaction(account.Id, TransactionType.Withdraw, amount, account.Balance, "Uttag"));
    }

    /// <summary>
    /// Överför pengar mellan två konton
    /// Skapar två transaktioner. En för utgående och en för inkommande
    /// </summary>
    /// <param name="fromAccountId"></param>
    /// <param name="toAccountId"></param>
    /// <param name="amount"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task TransferAsync(Guid fromAccountId, Guid toAccountId, decimal amount)
    {
        await EnsureInitializedAsync();

        if (fromAccountId == toAccountId)
        {
            throw new InvalidOperationException("Du kan inte överföra till samma konto");
        }

        var from = _accounts.FirstOrDefault(a => a.Id == fromAccountId);
        var to = _accounts.FirstOrDefault(a => a.Id == toAccountId);

        if (from == null || to == null)
        {
            throw new InvalidOperationException("Konto hittades inte");
        }
        
        from.Withdraw(amount);
        to.Deposit(amount);
        await SaveAsync();
        
        await _transactionService.AddTransactionAsync(
            new Transaction(from.Id, TransactionType.Transfer, amount, from.Balance, $"Till {to.Name}"));
        
        await _transactionService.AddTransactionAsync(
            new Transaction(to.Id, TransactionType.Transfer, amount, to.Balance, $"Från {from.Name}"));
        
        Console.WriteLine($"[AccountService] Överförde {amount} {from.Currency} från {from.Name} till {to.Name}");
    }
}