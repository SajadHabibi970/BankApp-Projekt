using System.Text.Json.Serialization;
using BlazorApp2.Interfaces;

namespace BlazorApp2.Domain;
/// <summary>
/// Representerar en enskild transaktion kopplad till ett bankkonto
/// Innehåller information om belopp, datum och saldo efter transaktionen
/// </summary>
public class Transaction : ITransaction
{
    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public DateTime Date { get; private set; }
    public TransactionType Type { get; private set; }
    public decimal Amount { get; private set; }
    public decimal BalanceAfter { get; private set; }
    public string Description { get; set; }

    /// <summary>
    /// Konstruktor som används när data laddas från localstorage
    /// </summary>
    /// <param name="id"></param>
    /// <param name="accountId"></param>
    /// <param name="date"></param>
    /// <param name="type"></param>
    /// <param name="amount"></param>
    /// <param name="balanceAfter"></param>
    /// <param name="description"></param>
    [JsonConstructor]
    public Transaction(Guid id, Guid accountId, DateTime date, TransactionType type, decimal amount,
        decimal balanceAfter
        , string description)
    {
        Id = id;
        AccountId = accountId;
        Date = date;
        Type = type;
        Amount = amount;
        BalanceAfter = balanceAfter;
        Description = description;
    }

    /// <summary>
    /// Används när en ny transaktion skapas i programmet
    /// Id genereras automatiskt och datum sätts nuvarande tidpunkt
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="type"></param>
    /// <param name="amount"></param>
    /// <param name="balanceAfter"></param>
    /// <param name="description"></param>
    public Transaction(Guid accountId, TransactionType type, decimal amount, decimal balanceAfter, string description = "")
    {
        AccountId = accountId;
        Type = type;
        Amount = amount;
        BalanceAfter = balanceAfter;
        Description = description;
        Date = DateTime.Now;
    }
    
    /// <summary>
    /// Tom standardkonstruktor kan behövs vid viss serialisering eller ramverk
    /// </summary>
    public Transaction() {}
}