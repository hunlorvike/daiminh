using Newtonsoft.Json;

namespace shared.Extensions;

/// <summary>
/// Provides extension methods for serializing objects to JSON and deserializing JSON strings to objects.
/// Uses Newtonsoft.Json (JSON.NET) for serialization and deserialization.
/// </summary>
public static class JsonExtensions
{
    /// <summary>
    /// Deserializes a JSON string to an object of the specified type.
    /// </summary>
    /// <typeparam name="TDataType">The type of object to deserialize to.</typeparam>
    /// <param name="stringJson">The JSON string to deserialize.</param>
    /// <returns>The deserialized object, or the default value for the type if the input string is null or empty.</returns>
    public static TDataType FromJson<TDataType>(this string stringJson)
    {
        if (string.IsNullOrEmpty(stringJson)) return default!;

        JsonSerializerSettings settings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        return JsonConvert.DeserializeObject<TDataType>(stringJson, settings)!;
    }

    /// <summary>
    /// Serializes an object to a JSON string.
    /// </summary>
    /// <typeparam name="TDataType">The type of object to serialize.</typeparam>
    /// <param name="data">The object to serialize.</param>
    /// <returns>The JSON string representation of the object, or "{}" if the input object is null.</returns>
    public static string ToJson<TDataType>(this TDataType data)
    {
        if (data == null) return "{}";

        JsonSerializerSettings settings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        return JsonConvert.SerializeObject(data, Formatting.None, settings);
    }
}