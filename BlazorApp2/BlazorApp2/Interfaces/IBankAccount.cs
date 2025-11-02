using BlazorApp2.Domain;

namespace BlazorApp2.Interfaces;

public interface IBankAccount
{
    Guid Id { get; }
    string Name { get; set; }
    AccountType AccountType { get; set; }
    decimal Balance { get; }
    string Currency { get; set; }
    DateTime LastUpdated { get; }
    
    void Deposit(decimal amount);
    
    void Withdraw(decimal amount);
}