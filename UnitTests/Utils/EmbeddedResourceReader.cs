using System.Reflection;
using System.Text.Json;

namespace AuroraScienceHub.Geopack.UnitTests.Utils;

/// <summary>
/// Helper class for reading embedded resources
/// </summary>
public static class EmbeddedResourceReader
{
    /// <summary>
    /// Reads the text content of the embedded resource
    /// </summary>
    public static async Task<string> ReadTextAsync(string resourceFilePath)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        await using Stream stream = assembly.GetManifestResourceStream(resourceFilePath)
                                    ?? throw new FileNotFoundException($"Embedded resource '{resourceFilePath}' not found.");

        using StreamReader reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }

    /// <summary>
    /// Reads the binary content of the embedded resource
    /// </summary>
    public static async Task<byte[]> ReadBinaryAsync(string resourceFilePath)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        await using Stream stream = assembly.GetManifestResourceStream(resourceFilePath)
                                    ?? throw new FileNotFoundException($"Embedded resource '{resourceFilePath}' not found.");

        using MemoryStream memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream).ConfigureAwait(false);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Reads the json content of the embedded resource
    /// </summary>
    public static async Task<T> ReadJsonAsync<T>(string resourceFilePath, JsonSerializerOptions? options = null)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        await using Stream stream = assembly.GetManifestResourceStream(resourceFilePath)
                                    ?? throw new FileNotFoundException($"Embedded resource '{resourceFilePath}' not found.");

        using StreamReader reader = new StreamReader(stream);
        string json = await reader.ReadToEndAsync().ConfigureAwait(false);
        return JsonSerializer.Deserialize<T>(json, options)
               ?? throw new InvalidOperationException("Could not deserialize JSON.");
    }
}
