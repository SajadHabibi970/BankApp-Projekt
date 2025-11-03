namespace BlazorApp2.Interfaces;
/// <summary>
/// Definierar metod för lokal lagring och hämtning av data
/// </summary>
public interface IStorageService
{
    Task SaveAsync<T>(string key, T value);
    Task<T?> LoadAsync<T>(string key);
}