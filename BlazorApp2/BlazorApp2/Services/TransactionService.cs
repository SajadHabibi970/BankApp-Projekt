namespace BlazorApp2.Services;
using BlazorApp2.Domain;
using BlazorApp2.Interfaces;

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

    private async Task SaveAsync()
    {
        await _storageService.SaveAsync(StorageKey, _transactions);
        Console.WriteLine($"[TransactionService] Sparade {_transactions.Count} transaktioner till LocalStorage");
    }

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

    public async Task<List<Transaction>> GetAllAsync()
    {
        await EnsureInitializedAsync();
        return _transactions
            .OrderByDescending(t => t.Date)
            .ToList();
    }

    public async Task<List<Transaction>> GetByAccountIdAsync(Guid accountId)
    {
        await EnsureInitializedAsync();
        return _transactions
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.Date)
            .ToList();
    }
}