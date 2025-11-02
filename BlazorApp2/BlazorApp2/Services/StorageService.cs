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
        var json = JsonSerializer.Serialize(value, _options);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, json);
    }

    public async Task<T?> LoadAsync<T>(string key)
    {
        var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
        return string.IsNullOrWhiteSpace(json)
            ? default
            : JsonSerializer.Deserialize<T>(json, _options);
    }
}