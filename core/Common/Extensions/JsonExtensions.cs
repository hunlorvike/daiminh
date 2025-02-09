using Newtonsoft.Json;

namespace core.Common.Extensions;

public static class JsonExtensions
{
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