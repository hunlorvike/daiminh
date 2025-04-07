using Minio;
using Minio.DataModel.Args;
using shared.Models;

namespace web.Areas.Admin.Services;

public class MinioStorageService : IMinioStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;
    private readonly string _publicBaseUrl;
    private readonly ILogger<MinioStorageService> _logger;

    public MinioStorageService(IConfiguration configuration, ILogger<MinioStorageService> logger)
    {
        _logger = logger;

        var minioConfig = configuration.GetSection("Minio");
        var endpoint = minioConfig["Endpoint"] ?? throw new ArgumentNullException("Minio:Endpoint");
        var accessKey = minioConfig["AccessKey"] ?? throw new ArgumentNullException("Minio:AccessKey");
        var secretKey = minioConfig["SecretKey"] ?? throw new ArgumentNullException("Minio:SecretKey");

        _bucketName = minioConfig["BucketName"] ?? throw new ArgumentNullException("Minio:BucketName");
        _publicBaseUrl = minioConfig["PublicBaseUrl"]?.TrimEnd('/') ?? "";

        var useSsl = minioConfig.GetValue<bool>("UseSSL");

        try
        {
            var clientBuilder = new MinioClient()
                   .WithEndpoint(endpoint)
                   .WithCredentials(accessKey, secretKey);
            if (useSsl)
            {
                clientBuilder.WithSSL();
            }
            else
            {
                // clientBuilder.WithHttpClient(new HttpClient(new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true }));
            }

            _minioClient = clientBuilder.Build();

            _logger.LogInformation("MinIO Client configured for endpoint: {Endpoint}, bucket: {BucketName}", endpoint, _bucketName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error configuring MinIO client.");
            throw;
        }
    }

    public async Task EnsureBucketExistsAsync()
    {
        try
        {
            var foundArgs = new BucketExistsArgs().WithBucket(_bucketName);
            bool found = await _minioClient.BucketExistsAsync(foundArgs);
            if (!found)
            {
                _logger.LogInformation("Bucket '{BucketName}' not found. Attempting to create it.", _bucketName);
                var makeArgs = new MakeBucketArgs().WithBucket(_bucketName);
                await _minioClient.MakeBucketAsync(makeArgs);
                _logger.LogInformation("Bucket '{BucketName}' created successfully.", _bucketName);

                string policyJson = $@"{{""Version"":""2012-10-17"",""Statement"":[{{""Effect"":""Allow"",""Principal"":{{""AWS"":[""*""]}},""Action"":[""s3:GetObject""],""Resource"":[""arn:aws:s3:::{_bucketName}/*""]}}]}}";
                var policyArgs = new SetPolicyArgs().WithBucket(_bucketName).WithPolicy(policyJson);
                await _minioClient.SetPolicyAsync(policyArgs);
                _logger.LogInformation("Public read policy set for bucket '{BucketName}'.", _bucketName);
            }
            else
            {
                _logger.LogDebug("Bucket '{BucketName}' already exists.", _bucketName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ensuring MinIO bucket '{BucketName}' exists.", _bucketName);
        }
    }
    public async Task<MinioUploadResult?> UploadFileAsync(IFormFile file, string? subFolder = null, string? customFileName = null)
    {
        if (file == null || file.Length == 0)
        {
            _logger.LogWarning("UploadFileAsync called with null or empty file.");
            return null;
        }

        try
        {
            await EnsureBucketExistsAsync();

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var uniqueFileName = customFileName ?? $"{Guid.NewGuid()}{fileExtension}";

            var objectName = string.IsNullOrWhiteSpace(subFolder)
                ? uniqueFileName
                : $"{subFolder.Trim('/')}/{uniqueFileName}";

            _logger.LogInformation("Attempting to upload object '{ObjectName}' to bucket '{BucketName}'.", objectName, _bucketName);

            using var stream = file.OpenReadStream();
            var putObjectArgs = new PutObjectArgs()
               .WithBucket(_bucketName)
               .WithObject(objectName)
               .WithStreamData(stream)
               .WithObjectSize(file.Length)
               .WithContentType(file.ContentType);

            var result = await _minioClient.PutObjectAsync(putObjectArgs);

            _logger.LogInformation("Successfully uploaded object '{ObjectName}' (ETag: {ETag}).", objectName, result.Etag);

            return new MinioUploadResult
            {
                ObjectName = objectName,
                FileSize = file.Length,
                ContentType = file.ContentType,
                OriginalFileName = file.FileName,
                GeneratedFileName = uniqueFileName,
                FileExtension = fileExtension,
                PublicUrl = GetPublicUrl(objectName)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file '{FileName}' to MinIO bucket '{BucketName}'.", file.FileName, _bucketName);
            return null;
        }
    }

    public async Task DeleteFileAsync(string objectName)
    {
        if (string.IsNullOrWhiteSpace(objectName))
        {
            _logger.LogWarning("DeleteFileAsync called with empty object name.");
            return;
        }

        try
        {
            await EnsureBucketExistsAsync();
            _logger.LogInformation("Attempting to delete object '{ObjectName}' from bucket '{BucketName}'.", objectName, _bucketName);

            var args = new RemoveObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName);
            await _minioClient.RemoveObjectAsync(args);
            _logger.LogInformation("Successfully deleted object '{ObjectName}'.", objectName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting object '{ObjectName}' from MinIO bucket '{BucketName}'.", objectName, _bucketName);
        }
    }

    public async Task DeleteFolderAsync(string folderPath)
    {
        if (string.IsNullOrWhiteSpace(folderPath))
        {
            _logger.LogWarning("DeleteFolderAsync called with empty folder path.");
            return;
        }

        try
        {
            await EnsureBucketExistsAsync();
            var objectPrefix = folderPath.TrimEnd('/') + "/";

            _logger.LogInformation("Attempting to delete objects with prefix '{Prefix}' from bucket '{BucketName}'.", objectPrefix, _bucketName);

            var listArgs = new ListObjectsArgs()
               .WithBucket(_bucketName)
               .WithPrefix(objectPrefix)
               .WithRecursive(true);

            var objectsToDelete = new List<string>();
            await foreach (var item in _minioClient.ListObjectsEnumAsync(listArgs))
            {
                if (!item.IsDir)
                {
                    objectsToDelete.Add(item.Key);
                }
            }

            if (objectsToDelete.Any())
            {
                _logger.LogDebug("Found {Count} objects to delete with prefix '{Prefix}'.", objectsToDelete.Count, objectPrefix);
                var deleteArgs = new RemoveObjectsArgs()
                    .WithBucket(_bucketName)
                    .WithObjects(objectsToDelete);

                var deleteErrors = await _minioClient.RemoveObjectsAsync(deleteArgs);
                foreach (var error in deleteErrors)
                {
                    _logger.LogWarning("Error deleting object '{ObjectName}' during folder delete: {Error}", error.Key, error.Message);
                }
                _logger.LogInformation("Finished deleting objects with prefix '{Prefix}'. Check logs for individual errors.", objectPrefix);
            }
            else
            {
                _logger.LogInformation("No objects found with prefix '{Prefix}' to delete.", objectPrefix);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting objects with prefix '{Prefix}' from bucket '{BucketName}'.", folderPath.TrimEnd('/') + "/", _bucketName);
        }
    }

    public string GetPublicUrl(string objectName)
    {
        if (string.IsNullOrEmpty(_publicBaseUrl) || string.IsNullOrEmpty(objectName))
        {
            return string.Empty;
        }
        return $"{_publicBaseUrl}/{objectName.TrimStart('/')}";
    }


    public async Task<string> GetPresignedUrlAsync(string objectName, int expiryInSeconds = 60 * 60)
    {
        try
        {
            await EnsureBucketExistsAsync();
            var args = new PresignedGetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithExpiry(expiryInSeconds);
            return await _minioClient.PresignedGetObjectAsync(args);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating presigned URL for object '{ObjectName}'.", objectName);
            return string.Empty;
        }
    }
}
