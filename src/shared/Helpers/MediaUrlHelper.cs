using Microsoft.Extensions.Configuration;

namespace shared.Helpers;

public static class MediaUrlHelper
{
    private static string _minioPublicBaseUrl = string.Empty;

    public static void Initialize(IConfiguration configuration)
    {
        _minioPublicBaseUrl = configuration.GetValue<string>("Minio:PublicBaseUrl")?.TrimEnd('/') ?? "";
    }

    public static string GetMinioUrl(string? objectPath, string placeholder = "/img/placeholder.svg")
    {
        if (!string.IsNullOrEmpty(objectPath) && !string.IsNullOrEmpty(_minioPublicBaseUrl))
        {
            return $"{_minioPublicBaseUrl}/{objectPath.TrimStart('/')}";
        }
        return placeholder;
    }

}
