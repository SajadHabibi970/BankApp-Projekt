using BlazorApp2.Domain;

namespace BlazorApp2.Interfaces;

public interface IBankAccount
{
    Guid Id { get; }
    string Name { get; }
    AccountType AccountType { get; }
    decimal Balance { get; }
    string Currency { get; }
    DateTime LastUpdated { get; }
    
    void Deposit(decimal amount);
    
    void Withdraw(decimal amount);
}