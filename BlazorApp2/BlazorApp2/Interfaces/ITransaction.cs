namespace BlazorApp2.Interfaces;
using BlazorApp2.Domain;

public interface ITransaction
{
    Guid Id { get; }
    Guid AccountId { get; }
    DateTime Date { get; }
    TransactionType Type { get; }
    decimal Amount { get; }
    decimal BalanceAfter { get; } 
    string Description { get; }
}