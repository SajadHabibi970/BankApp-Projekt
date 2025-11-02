using System.Text.Json.Serialization;
using BlazorApp2.Interfaces;

namespace BlazorApp2.Domain;

public class Transaction : ITransaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AccountId { get; set; }
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public decimal BalanceAfter { get; set; }
    public string Description { get; set; }
    
    [JsonConstructor]
    public Transaction() { }

    public Transaction(Guid accountId, TransactionType type, decimal amount, decimal balanceAfter, string description = "")
    {
        AccountId = accountId;
        Type = type;
        Amount = amount;
        BalanceAfter = balanceAfter;
        Description = description;
        Date = DateTime.Now;
    }
}