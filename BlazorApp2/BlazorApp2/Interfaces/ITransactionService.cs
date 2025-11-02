namespace BlazorApp2.Interfaces;
using BlazorApp2.Domain;

public interface ITransactionService
{
    Task AddTransactionAsync(Transaction transaction);
    Task<List<Transaction>> GetAllAsync();
    Task<List<Transaction>> GetByAccountIdAsync(Guid accountId);
}