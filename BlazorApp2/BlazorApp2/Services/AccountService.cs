using BlazorApp2.Domain;
using BlazorApp2.Interfaces;

namespace BlazorApp2.Services;

public class AccountService : IAccountService
{
    private readonly List<BankAccount> _accounts = new();

    public IBankAccount CreateBankAccount(string name, AccountType accountType, string currency, decimal initialBalance)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Kontonamn får inte vara tomt");

        if (initialBalance < 0)
            throw new ArgumentException("Startsaldo kan inte vara negativt");

        if (string.IsNullOrWhiteSpace(currency))
            currency = "SEK";

        var account = new BankAccount( name, accountType, initialBalance, currency);
        _accounts.Add(account);

        Console.WriteLine($"[AccountService] Skapade konto {name} ({accountType}) med {initialBalance} {currency}");

        return account;
    }

    public List<IBankAccount> GetAccounts()
    {
        return _accounts.Cast<IBankAccount>().ToList();
    }
}