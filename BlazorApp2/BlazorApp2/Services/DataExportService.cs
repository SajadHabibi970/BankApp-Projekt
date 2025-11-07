using System.Text.Json;
using BlazorApp2.Domain;
using BlazorApp2.Interfaces;

namespace BlazorApp2.Services;
/// <summary>
/// Hanterar export och import av all applikationsdata
/// Sparar konton och transaktioner till JSON och återställer dem senare
/// </summary>
public class DataExportService
{
    private readonly IStorageService _storageService;
    private readonly IAccountService  _accountService;
    private readonly ITransactionService _transactionService;

    // Inställningar för JSON serialisernig
    private static readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Konstruktor som tar in de services vi behöver
    /// </summary>
    /// <param name="storageService"></param>
    /// <param name="accountService"></param>
    /// <param name="transactionService"></param>
    public DataExportService(IStorageService storageService ,IAccountService accountService, ITransactionService transactionService)
    {
         _storageService = storageService;
        _accountService = accountService;
        _transactionService = transactionService;
    }

    /// <summary>
    /// Exporterar alla konton och transaktioner till en JSON sträng som
    /// kan användas för backup eller flytt av data
    /// </summary>
    /// <returns>JSON strängen som innehåller konton och transaktioner</returns>
    public async Task<string> ExportAsync()
    {
        var accounts = await _accountService.GetAccounts();
        var transactions = await _transactionService.GetAllAsync();

        var exportData = new ExportData
        {
            ExportDate = DateTime.Now,
            Accounts = accounts.Cast<BankAccount>().ToList(),
            Transactions = transactions
        };
        return JsonSerializer.Serialize(exportData, _options);
    }

    /// <summary>
    /// Importerar JSON data och återskapar konton och transaktioner
    /// </summary>
    /// <param name="json"> JSON text som innehåller exportdata</param>
    public async Task ImportAsync(string json)
    {
        try
        {
            var data = JsonSerializer.Deserialize<ExportData>(json, _options);

            if (data == null)
            {
                Console.WriteLine("[DataExportService] Ingen giltig data hittades vid import");
                return;
            }

            if (data.Accounts != null && data.Accounts.Any())
            {
                await _storageService.SaveAsync("bank-accounts", data.Accounts);
                Console.WriteLine($"[DataExportService] Importerade {data.Accounts.Count} konton");
            }

            if (data.Transactions != null && data.Transactions.Any())
            {
                await _storageService.SaveAsync("bank-transactions", data.Transactions);
                Console.WriteLine($"[DataExportService] Importerade {data.Transactions.Count} transaktioner");
            }
            Console.WriteLine("[DataExportService] Import klar");
        }
        
        catch (Exception e)
        {
            Console.WriteLine($"[DataExportService] Fel vid import: {e.Message}");
        }
    }

    /// <summary>
    /// Representerar strukturen av export och import filen
    /// innehåller alla konton och transaktioner
    /// </summary>
    private class ExportData
    {
        public DateTime ExportDate { get; set; }
        public List<BankAccount> Accounts { get; set; }
        public List<Transaction> Transactions { get; set; } = new();
    }
}