using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.Credentials;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Portal.Files.Api.Models;
using Portal.Files.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AwsConfig>(builder.Configuration.GetSection("Aws"));
builder.Services.Configure<PortalConfig>(builder.Configuration.GetSection("Portal"));
builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    AwsConfig aws = sp.GetRequiredService<IOptions<AwsConfig>>().Value;

    AWSCredentials baseCredentials = DefaultAWSCredentialsIdentityResolver.GetCredentials();
    
    var credentials = new AssumeRoleAWSCredentials(
        baseCredentials,
        aws.RoleArn,
        "portal-iam-lab-session"
    );
    
     return new AmazonS3Client(
        credentials,
        RegionEndpoint.GetBySystemName(aws.Region)
    );

});
builder.Services.AddScoped<IFileService, S3FileService>();


var app = builder.Build();

app.MapGet("/files", async ([FromServices]IFileService fileService, CancellationToken ctx) =>
{
    return Results.Ok(await fileService.ListAsync(cancellationToken: ctx));
});

app.Run();