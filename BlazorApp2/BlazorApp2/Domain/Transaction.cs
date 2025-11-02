using System.Text.Json.Serialization;
using BlazorApp2.Interfaces;

namespace BlazorApp2.Domain;

public class Transaction : ITransaction
{
    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public DateTime Date { get; private set; }
    public TransactionType Type { get; private set; }
    public decimal Amount { get; private set; }
    public decimal BalanceAfter { get; private set; }
    public string Description { get; set; }

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

    public Transaction(Guid accountId, TransactionType type, decimal amount, decimal balanceAfter, string description = "")
    {
        AccountId = accountId;
        Type = type;
        Amount = amount;
        BalanceAfter = balanceAfter;
        Description = description;
        Date = DateTime.Now;
    }
    
    public Transaction() {}
}