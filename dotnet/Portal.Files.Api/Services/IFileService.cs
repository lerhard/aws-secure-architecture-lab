namespace Portal.Files.Api.Services;

public interface IFileService
{
   Task<IReadOnlyCollection<string>> ListAsync(CancellationToken cancellationToken = default);
    
}