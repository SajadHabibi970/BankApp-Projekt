namespace BlazorApp2.Services;
using BlazorApp2.Domain;
using BlazorApp2.Interfaces;

public class TransactionService : ITransactionService
{
    private const string StorageKey = "bank-transactions";
    private readonly IStorageService _storage;
    private readonly List<Transaction> _transactions = new();
    private bool _loaded;

    public TransactionService(IStorageService storage)
    {
        _storage = storage;
    }

    private async Task EnsureLoadedAsync()
    {
        if (_loaded) return;

        var fromStorage = await _storage.LoadAsync<List<Transaction>>(StorageKey);
        if (fromStorage is { Count: > 0 })
            _transactions.AddRange(fromStorage);

        _loaded = true;
    }

    private async Task SaveAsync() =>
        await _storage.SaveAsync(StorageKey, _transactions);

    public async Task AddTransactionAsync(Transaction transaction)
    {
        await EnsureLoadedAsync();
        _transactions.Add(transaction);
        await SaveAsync();
        Console.WriteLine($"[TransactionService] Ny transaktion: {transaction.Type} {transaction.Amount} kr");
    }

    public async Task<List<Transaction>> GetAllAsync()
    {
        await EnsureLoadedAsync();
        return _transactions.OrderByDescending(t => t.Date).ToList();
    }

    public async Task<List<Transaction>> GetByAccountIdAsync(Guid accountId)
    {
        await EnsureLoadedAsync();
        return _transactions
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.Date)
            .ToList();
    }
}