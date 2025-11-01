using BlazorApp2.Interfaces;

namespace BlazorApp2.Domain;

public class BankAccount : IBankAccount
{
    public Guid Id { get; private set; }
    public string Name { get; set; }
    public AccountType AccountType { get; set; }
    public decimal Balance { get; private set; }
    public string Currency { get; set; }
    public DateTime LastUpdated { get; private set; }
    
    public BankAccount (string name, AccountType accountType, decimal initialBalance, string currency)
    {
        Id = Guid.NewGuid();
        Name = name;
        AccountType = accountType;
        Balance = initialBalance;
        Currency = currency;
        LastUpdated = DateTime.Now;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Belopp måste vara större än 0");
        }
        Balance += amount;
        LastUpdated = DateTime.Now;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Belopp måste vara större än 0");
        }

        if (amount > Balance)
        {
            throw new InvalidOperationException("Otillräckligt saldo");
        }
        Balance -= amount;
        LastUpdated = DateTime.Now;
    }
}