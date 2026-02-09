using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using Portal.Files.Api.Models;

namespace Portal.Files.Api.Services;

public class S3FileService : IFileService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucket;
    
    public S3FileService(IAmazonS3 s3Client, IOptions<PortalConfig> portalConfig)
    {
        _s3Client = s3Client;
        _bucket = portalConfig.Value.Bucket;
    }
    
    public async Task<IReadOnlyCollection<string>> ListAsync(CancellationToken cancellationToken = default)
    {
        ListObjectsV2Response response = await _s3Client.ListObjectsV2Async(new ListObjectsV2Request()
        {
            BucketName = _bucket
        }, cancellationToken);

        if (response.S3Objects is null)
        {
            return [];
        }
        
        return response.S3Objects.Select(x => x.Key).ToArray();
    }
}