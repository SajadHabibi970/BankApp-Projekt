using BlazorApp2.Domain;
using BlazorApp2.Interfaces;

namespace BlazorApp2.Services;

public class AccountService : IAccountService
{
    private const string StorageKey = "bank-accounts";
    private readonly IStorageService _storage;
    private readonly List<BankAccount> _accounts = new();
    private bool _isLoaded = false;

    public AccountService(IStorageService storage)
    {
        _storage = storage;
    }

    private async Task EnsureLoadedAsync()
    {
        if (_isLoaded) return;

        var fromStorage = await _storage.LoadAsync<List<BankAccount>>(StorageKey);
        if (fromStorage is { Count: > 0 })
        {
            _accounts.AddRange(fromStorage);
        }

        _isLoaded = true;
    }

    private async Task SaveAsync() =>
        await _storage.SaveAsync(StorageKey, _accounts);

    public async Task<IBankAccount> CreateBankAccount(string name, AccountType accountType, string currency, decimal initialBalance)
    {
        await EnsureLoadedAsync();

        var account = new BankAccount(name, accountType, initialBalance, currency);
        _accounts.Add(account);
        await SaveAsync();
        return account;
    }

    public async Task<List<IBankAccount>> GetAccounts()
    {
        await EnsureLoadedAsync();
        return _accounts.Cast<IBankAccount>().ToList();
    }

    public async Task<IBankAccount?> GetAccount(Guid accountId)
    {
        await EnsureLoadedAsync();
        return _accounts.FirstOrDefault(a => a.Id == accountId);
    }
}