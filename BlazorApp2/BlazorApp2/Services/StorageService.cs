namespace BlazorApp2.Services;
using System.Text.Json;
using BlazorApp2.Interfaces;
using Microsoft.JSInterop;

public class StorageService : IStorageService
{
    private readonly IJSRuntime _jsRuntime;
    
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true,
        IncludeFields = true
    };

    public StorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SaveAsync<T>(string key, T value)
    {
        try
        {
            var json = JsonSerializer.Serialize(value, _options);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, json);
            Console.WriteLine($"[StorageService] Sparade data till {key}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"[StorageService] Fel vid sparning av {key}: {e.Message}");
        }
    }

    public async Task<T?> LoadAsync<T>(string key)
    {
        try
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);

            if (string.IsNullOrWhiteSpace(json))
            {
                Console.WriteLine($"[StorageService] Ingen data hittades för {key}");
                return default;
            }
            
            var result = JsonSerializer.Deserialize<T>(json, _options);
            Console.WriteLine($"[StorageService] Laddade data för {key}");
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine($"[StorageService] Fel vid laddning av {key}: {e.Message}");
            return default;
        }
    }
}