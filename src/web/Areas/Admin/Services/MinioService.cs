using AutoRegister;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using web.Areas.Admin.Services.Interfaces;

namespace web.Areas.Admin.Services;

[Register(ServiceLifetime.Scoped)]
public class MinioService : IMinioService
{
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioService> _logger;

    public MinioService(IMinioClient minioClient, ILogger<MinioService> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task CreateBucketIfNotExistsAsync(string bucketName)
    {
        try
        {
            var beArgs = new BucketExistsArgs().WithBucket(bucketName);
            bool found = await _minioClient.BucketExistsAsync(beArgs).ConfigureAwait(false);
            if (!found)
            {
                var mbArgs = new MakeBucketArgs().WithBucket(bucketName);
                await _minioClient.MakeBucketAsync(mbArgs).ConfigureAwait(false);
                _logger.LogInformation("Bucket {BucketName} created successfully.", bucketName);
            }
            else
            {
                _logger.LogInformation("Bucket {BucketName} already exists.", bucketName);
            }
        }
        catch (MinioException ex)
        {
            _logger.LogError(ex, "Error with MinIO bucket operations: {Message}", ex.Message);
            throw;
        }
    }

    public async Task<string> UploadFileAsync(string bucketName, string objectName, Stream data, string contentType)
    {
        try
        {
            data.Seek(0, SeekOrigin.Begin);
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithStreamData(data)
                .WithObjectSize(data.Length)
                .WithContentType(contentType);

            var response = await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
            return response.Etag;
        }
        catch (MinioException ex)
        {
            _logger.LogError(ex, "Error uploading file to MinIO: {Message}", ex.Message);
            throw;
        }
    }

    public async Task<bool> FileExistsAsync(string bucketName, string objectName)
    {
        try
        {
            var statObjectArgs = new StatObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName);
            await _minioClient.StatObjectAsync(statObjectArgs).ConfigureAwait(false);
            return true;
        }
        catch (ObjectNotFoundException)
        {
            return false;
        }
        catch (MinioException ex)
        {
            _logger.LogError(ex, "Error checking file existence in MinIO: {Message}", ex);
            return false;
        }
    }

    public async Task<Stream> GetFileAsync(string bucketName, string objectName)
    {
        try
        {
            var memoryStream = new MemoryStream();
            var getObjectArgs = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithCallbackStream((stream) =>
                {
                    stream.CopyTo(memoryStream);
                });

            await _minioClient.GetObjectAsync(getObjectArgs).ConfigureAwait(false);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        catch (MinioException ex)
        {
            _logger.LogError(ex, "Error getting file from MinIO: {Message}", ex);
            throw;
        }
    }

    public async Task<bool> DeleteFileAsync(string bucketName, string objectName)
    {
        try
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName);
            await _minioClient.RemoveObjectAsync(removeObjectArgs).ConfigureAwait(false);
            return true;
        }
        catch (MinioException ex)
        {
            _logger.LogError(ex, "Error deleting file from MinIO: {Message}", ex);
            return false;
        }
    }

    public async Task<string> GetFilePresignedUrlAsync(string bucketName, string objectName, int expiresInt = 3600) // Default 1 hour
    {
        try
        {
            var presignedGetObjectArgs = new PresignedGetObjectArgs()
                                        .WithBucket(bucketName)
                                        .WithObject(objectName)
                                        .WithExpiry(expiresInt);

            return await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs).ConfigureAwait(false);
        }
        catch (MinioException ex)
        {
            _logger.LogError(ex, "Error generating presigned URL for {ObjectName}: {Message}", objectName, ex);
            return string.Empty;
        }
    }
}
