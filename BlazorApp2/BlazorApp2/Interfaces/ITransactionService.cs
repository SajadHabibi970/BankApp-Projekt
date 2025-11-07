namespace BlazorApp2.Interfaces;
using BlazorApp2.Domain;
/// <summary>
/// Tjänst för hantering av transaktioner
/// </summary>
public interface ITransactionService
{
    Task AddTransactionAsync(Transaction transaction);
    Task<List<Transaction>> GetAllAsync();
    Task<List<Transaction>> GetByAccountIdAsync(Guid accountId);
}