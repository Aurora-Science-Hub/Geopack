using System.Reflection;

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
}
