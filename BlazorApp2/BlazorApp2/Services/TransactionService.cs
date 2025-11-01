namespace BlazorApp2.Services;
using BlazorApp2.Domain;
using BlazorApp2.Interfaces;

public class TransactionService : ITransactionService
{
    private readonly List<Transaction> _transactions = new();
    
    public void AddTransaction(Transaction transaction)
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));

        _transactions.Add(transaction);
        Console.WriteLine($"[TransactionService] Ny transaktion: {transaction.Type} {transaction.Amount} kr");
    }
    
    public List<Transaction> GetAll()
    {
        return _transactions
            .OrderByDescending(t => t.Date)
            .ToList();
    }
    
    public List<Transaction> GetByAccountId(Guid accountId)
    {
        return _transactions
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.Date)
            .ToList();
    }
}