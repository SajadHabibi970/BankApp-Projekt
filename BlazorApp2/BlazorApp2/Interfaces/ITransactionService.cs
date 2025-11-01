namespace BlazorApp2.Interfaces;
using BlazorApp2.Domain;

public interface ITransactionService
{
    void AddTransaction(Transaction transaction);
    List<Transaction> GetAll();
    List<Transaction> GetByAccountId(Guid accountId);
}