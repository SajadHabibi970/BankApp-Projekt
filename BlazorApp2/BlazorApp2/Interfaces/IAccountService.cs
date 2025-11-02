using BlazorApp2.Domain;

namespace BlazorApp2.Interfaces;

public interface IAccountService
{
    Task<IBankAccount> CreateBankAccount(string name, AccountType accountType, string currency, decimal initialBalance);
    Task<List<IBankAccount>> GetAccounts();
    Task<IBankAccount?> GetAccount(Guid accountId);
}