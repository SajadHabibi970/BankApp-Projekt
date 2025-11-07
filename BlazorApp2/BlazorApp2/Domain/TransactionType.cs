namespace BlazorApp2.Domain;
/// <summary>
/// Representerar olika typer av transaktion på ett bankkonto
/// Används i klassen transaktion för att tydligt ange vilken typ av händelse det är
/// </summary>
public enum TransactionType
{
    Deposit,
    Withdraw,
    Transfer
}