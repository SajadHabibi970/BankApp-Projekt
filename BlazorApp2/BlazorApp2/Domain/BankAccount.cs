using System.Text.Json.Serialization;
using BlazorApp2.Interfaces;

namespace BlazorApp2.Domain;
/// <summary>
/// Representrerar ett bankkonto och hanterar de grundläggande funktioner
/// </summary>
public class BankAccount : IBankAccount
{
    public Guid Id { get; private set; }
    public string Name { get; set; }
    public AccountType AccountType { get; set; }
    public decimal Balance { get; private set; }
    public string Currency { get; set; }
    public DateTime LastUpdated { get; private set; }
    
    /// <summary>
    /// Konstruktor som används när ett nytt konto skapas
    /// Den genererar ett nytt unik Id och sätter startvärden för kontot
    /// </summary>
    /// <param name="name"></param>
    /// <param name="accountType"></param>
    /// <param name="initialBalance"></param>
    /// <param name="currency"></param>
    /// <param name="lastUpdated"></param>
    public BankAccount (string name, AccountType accountType, decimal initialBalance, string currency, DateTime lastUpdated)
    {
        Id = Guid.NewGuid();
        Name = name;
        AccountType = accountType;
        Balance = initialBalance;
        Currency = currency;
        LastUpdated = lastUpdated;
    }

    /// <summary>
    /// Metod för att sätta in pengar på kontot
    /// Om Beloppet är 0 eller mindre kastas ett undantag
    /// </summary>
    /// <param name="amount"></param>
    /// <exception cref="ArgumentException"></exception>
    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Belopp måste vara större än 0");
        }
        Balance += amount;
        LastUpdated = DateTime.Now;
    }

    /// <summary>
    /// Metod för att ta ut pengar
    /// Kontrollerar att beloppet är positivt och att tillräckligt saldo finns
    /// </summary>
    /// <param name="amount"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
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

    /// <summary>
    /// Används vid laddning från localstorage
    /// [JsonConstructor] berättar för JSON hanteraren vilken konstruktor som ska användas
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="accountType"></param>
    /// <param name="balance"></param>
    /// <param name="currency"></param>
    [JsonConstructor]
    public BankAccount(Guid id, string name, AccountType accountType, decimal balance, string currency)
    {
        Id = id;
        Name = name;
        AccountType = accountType;
        Balance = balance;
        Currency = currency;
        LastUpdated = DateTime.Now;
    }
}