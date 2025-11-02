using BlazorApp2.Domain;
using BlazorApp2.Interfaces;

namespace BlazorApp2.Services;

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

    private async Task EnsureInitializedAsync()
    {
        if (_isLoaded) return;

        var fromStorage = await _storageService.LoadAsync<List<BankAccount>>(StorageKey);
        if (fromStorage is { Count: > 0 })
        {
            _accounts.AddRange(fromStorage);
            Console.WriteLine($"$[AccountService] laddade {_accounts.Count} konton från LocalStorage");
        }

        _isLoaded = true;
    }

    private async Task SaveAsync()
    {
        await _storageService.SaveAsync(StorageKey, _accounts);
    }

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

    public async Task<List<IBankAccount>> GetAccounts()
    {
        await EnsureInitializedAsync();
        return _accounts.Cast<IBankAccount>().ToList();
    }

    public async Task<IBankAccount?> GetAccount(Guid accountId)
    {
        await EnsureInitializedAsync();
        return _accounts.FirstOrDefault(a => a.Id == accountId);
    }

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
            new Transaction(from.Id,TransactionType.Transfer, amount,from.Balance,$"Till {to.Name}"));
        
        await _transactionService.AddTransactionAsync(
            new Transaction(to.Id,TransactionType.Deposit, amount,to.Balance,$"Från {from.Name}"));
        
        Console.WriteLine($"[AccountService] Överförde {amount} {from.Currency} från {from.Name} till {to.Name}");
    }
}