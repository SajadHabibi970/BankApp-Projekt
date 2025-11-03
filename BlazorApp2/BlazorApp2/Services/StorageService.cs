namespace BlazorApp2.Services;
using System.Text.Json;
using BlazorApp2.Interfaces;
using Microsoft.JSInterop;
/// <summary>
/// Hanterar lagring av data i webbläsarens localstorage
/// Sparar och hämtar information som konton och transaktioner mellan sessioner
/// </summary>
public class StorageService : IStorageService
{
    private readonly IJSRuntime _jsRuntime;
    
    // Insättning för JSON serialisering. Dessa styr hur objekt konverteras till text
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

    /// <summary>
    /// Sparar ett objekt till localstorage
    /// </summary>
    /// <param name="key">nyckeln som används för att spara datan i localstorage</param>
    /// <param name="value">Objektet som ska sparas</param>
    /// <typeparam name="T">Typen av objekt som ska anropas</typeparam>
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

    /// <summary>
    /// Hämtar ett objekt från localstorage
    /// Returnerar det deserialiserade objektet eller null om inget hittades
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
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