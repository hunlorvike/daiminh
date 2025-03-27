namespace web.Areas.Admin.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _environment;
    private readonly string _uploadsFolder;

    public LocalFileStorageService(IWebHostEnvironment environment)
    {
        _environment = environment;
        _uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

        // Ensure uploads folder exists
        if (!Directory.Exists(_uploadsFolder))
        {
            Directory.CreateDirectory(_uploadsFolder);
        }
    }

    public async Task<string> SaveFileAsync(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
        {
            return string.Empty;
        }

        // Create folder if it doesn't exist
        var folderPath = Path.Combine(_uploadsFolder, folder);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Generate unique filename
        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
        var filePath = Path.Combine(folderPath, fileName);

        // Save file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Return relative path for storage in database
        return $"/uploads/{folder}/{fileName}";
    }

    public Task DeleteFileAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return Task.CompletedTask;
        }

        // Convert relative path to absolute path
        var fullPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }
}