using AutoMapper;
using domain.Entities;
using shared.Enums;
using shared.Models;

namespace web.Areas.Admin.Resolvers;

public class MediaTypeResolver : IValueResolver<MinioUploadResult, MediaFile, MediaType>
{
    public MediaType Resolve(MinioUploadResult source, MediaFile destination, MediaType destMember, ResolutionContext context)
    {
        var mime = source.ContentType?.ToLowerInvariant() ?? "";
        var ext = source.FileExtension?.ToLowerInvariant() ?? "";

        if (mime.StartsWith("image/")) return MediaType.Image;
        if (mime.StartsWith("video/")) return MediaType.Video;
        if (mime.StartsWith("audio/")) return MediaType.Audio;

        if (mime.StartsWith("application/pdf") || mime.Contains("wordprocessingml") || mime.Contains("spreadsheetml") || mime.Contains("presentationml") || mime.Equals("text/plain") || mime.Equals("text/csv") || mime.Equals("application/msword") || mime.Equals("application/vnd.ms-excel") || mime.Equals("application/vnd.ms-powerpoint"))
            return MediaType.Document;

        if (mime.Equals("application/zip") || mime.Equals("application/x-rar-compressed") || mime.Equals("application/x-7z-compressed") || mime.Equals("application/gzip") || mime.Equals("application/x-tar"))
            return MediaType.Archive;

        // Fallback checks based on extension if MimeType is generic (like application/octet-stream)
        if (string.IsNullOrEmpty(mime) || mime == "application/octet-stream")
        {
            if (new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".svg", ".webp" }.Contains(ext)) return MediaType.Image;
            if (new[] { ".mp4", ".mov", ".avi", ".wmv", ".mkv", ".webm" }.Contains(ext)) return MediaType.Video;
            if (new[] { ".mp3", ".wav", ".ogg", ".m4a", ".flac" }.Contains(ext)) return MediaType.Audio;
            if (new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".txt", ".csv" }.Contains(ext)) return MediaType.Document;
            if (new[] { ".zip", ".rar", ".7z", ".gz", ".tar" }.Contains(ext)) return MediaType.Archive;
        }

        return MediaType.Other;
    }
}