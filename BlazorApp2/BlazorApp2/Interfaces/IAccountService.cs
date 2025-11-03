using BlazorApp2.Domain;

namespace BlazorApp2.Interfaces;
/// <summary>
/// Definierar tjänst för hantering av bankkonton
/// </summary>
public interface IAccountService
{
    Task<IBankAccount> CreateBankAccount(string name, AccountType accountType, string currency, decimal initialBalance);
    Task<List<IBankAccount>> GetAccounts();
    Task<IBankAccount?> GetAccount(Guid accountId);
    
    Task DepositAsync(Guid accountId, decimal amount);
    
    Task WithdrawAsync(Guid accountId, decimal amount);
    
    Task TransferAsync(Guid fromAccountId, Guid toAccountId, decimal amount);
    
    Task ApplyInterestAsync(decimal interestRate = 0.01m);
}