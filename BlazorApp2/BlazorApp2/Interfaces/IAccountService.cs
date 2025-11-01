using BlazorApp2.Domain;

namespace BlazorApp2.Interfaces;

public interface IAccountService
{
    IBankAccount CreateBankAccount(string name, AccountType accountType, string currency, decimal initialBalance);
    
    List<IBankAccount> GetAccounts();
}