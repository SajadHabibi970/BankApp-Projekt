namespace BlazorApp2.Interfaces;

public interface IStorageService
{
    Task SaveAsync<T>(string key, T value);
    Task<T?> LoadAsync<T>(string key);
}