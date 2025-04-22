
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace web.Areas.Admin.Services;

public class MinioService : IMinioService
{
    private readonly IMinioClient _minioClient;
    private readonly IConfiguration _configuration;
    private readonly string _defaultBucketName;
    private readonly string _publicBaseUrl;

    public MinioService(IMinioClient minioClient, IConfiguration configuration)
    {
        _minioClient = minioClient;
        _configuration = configuration;
        _defaultBucketName = _configuration["Minio:BucketName"] ?? throw new ArgumentNullException("Minio:BucketName configuration is missing");
        _publicBaseUrl = _configuration["Minio:PublicBaseUrl"] ?? "";
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

                var policy = string.Format(
                    "{{\"Version\":\"2012-10-17\",\"Statement\":[" +
                    "{{\"Effect\":\"Allow\",\"Principal\":\"*\",\"Action\":[\"s3:GetObject\"],\"Resource\":[\"arn:aws:s3:::{0}/*\"]}}," +
                    "{{\"Effect\":\"Allow\",\"Principal\":\"*\",\"Action\":[\"s3:ListBucket\"],\"Resource\":[\"arn:aws:s3:::{0}\"]}}" +
                    "]}}",
                    bucketName);
                var spArgs = new SetPolicyArgs().WithBucket(bucketName).WithPolicy(policy);
                await _minioClient.SetPolicyAsync(spArgs).ConfigureAwait(false);
            }
            else
            {
                Console.WriteLine($"Bucket {bucketName} already exists.");
            }
        }
        catch (MinioException)
        {
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
        catch (MinioException)
        {
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
        catch (MinioException)
        {
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
        catch (MinioException)
        {
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
        catch (MinioException)
        {
            return false;
        }
    }

    public string GetFilePublicUrl(string bucketName, string objectName)
    {
        if (string.IsNullOrEmpty(_publicBaseUrl))
        {
            throw new InvalidOperationException("Minio:PublicBaseUrl configuration is missing.");
        }
        var baseUrl = _publicBaseUrl.EndsWith("/") ? _publicBaseUrl : _publicBaseUrl + "/";
        var cleanedObjectName = objectName.StartsWith("/") ? objectName.Substring(1) : objectName;
        return $"{baseUrl}{cleanedObjectName}";
    }

    public async Task<string> GetFilePresignedUrlAsync(string bucketName, string objectName, int expiresInt = 300)
    {
        try
        {
            var presignedPutArgs = new PresignedGetObjectArgs()
                                        .WithBucket(bucketName)
                                        .WithObject(objectName)
                                        .WithExpiry(expiresInt);

            return await _minioClient.PresignedGetObjectAsync(presignedPutArgs).ConfigureAwait(false);
        }
        catch (MinioException)
        {
            throw;
        }
    }
}
