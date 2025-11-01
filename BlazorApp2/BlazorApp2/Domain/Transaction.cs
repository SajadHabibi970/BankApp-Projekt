using BlazorApp2.Interfaces;

namespace BlazorApp2.Domain;

public class Transaction : ITransaction
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid AccountId { get; private set; }
    public DateTime Date { get; private set; }
    public TransactionType Type { get; private set; }
    public decimal Amount { get; private set; }
    public decimal BalanceAfter { get; private set; }

    public Transaction(Guid accountId, TransactionType type, decimal amount, decimal balanceAfter)
    {
        AccountId = accountId;
        Type = type;
        Amount = amount;
        BalanceAfter = balanceAfter;
        Date = DateTime.Now;
    }
    
    public Transaction() { }
}