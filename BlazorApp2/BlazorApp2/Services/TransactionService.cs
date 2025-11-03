namespace BlazorApp2.Services;
using BlazorApp2.Domain;
using BlazorApp2.Interfaces;
/// <summary>
/// Hanterar alla transaktioner
/// Sparar, laddar och filterar transaktioner via localstorage
/// </summary>
public class TransactionService : ITransactionService
{
    private const string StorageKey = "bank-transactions";
    private readonly IStorageService _storageService;
    private readonly List<Transaction> _transactions = new();
    private bool _isLoaded;

    public TransactionService(IStorageService storageService)
    {
        _storageService = storageService;
    }
    /// <summary>
    /// Säkerställer att transaktioner har laddats från lagring
    /// </summary>
    private async Task EnsureInitializedAsync()
    {
        if (_isLoaded)
        {
            return;
        }

        var fromStorage = await _storageService.LoadAsync<List<Transaction>>(StorageKey);
        _transactions.Clear();

        if (fromStorage != null && fromStorage.Count > 0)
        {
          _transactions.AddRange(fromStorage);
          Console.WriteLine($"[TransactionService] Laddade {_transactions.Count} transaktioner från LocalStorage");
        }

        else
        {
            Console.WriteLine($"[TransactionService] Inga transaktioner hittades i LocalStorage");
        }
        
        _isLoaded = true;
    }

    /// <summary>
    /// Sparar alla transaktioner till localstorage
    /// </summary>
    private async Task SaveAsync()
    {
        await _storageService.SaveAsync(StorageKey, _transactions);
        Console.WriteLine($"[TransactionService] Sparade {_transactions.Count} transaktioner till LocalStorage");
    }

    /// <summary>
    /// Lägger till en ny transaktion och sparar i listan
    /// </summary>
    /// <param name="transaction"></param>
    public async Task AddTransactionAsync(Transaction transaction)
    {
       await EnsureInitializedAsync();

       if (transaction == null)
       {
           return;
       }
       
       _transactions.Add(transaction);
       await SaveAsync();
       Console.WriteLine($"[TransaktionService] Ny transaktion: {transaction.Type} {transaction.Amount}kr {transaction.Description}");
    }

    /// <summary>
    /// Hämtar all transaktioner, sorterade efter datum
    /// </summary>
    /// <returns></returns>
    public async Task<List<Transaction>> GetAllAsync()
    {
        await EnsureInitializedAsync();
        return _transactions
            .OrderByDescending(t => t.Date)
            .ToList();
    }

    /// <summary>
    /// Hämtar alla transaktioner som tillhör ett specifikt konto
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    public async Task<List<Transaction>> GetByAccountIdAsync(Guid accountId)
    {
        await EnsureInitializedAsync();
        return _transactions
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.Date)
            .ToList();
    }
}