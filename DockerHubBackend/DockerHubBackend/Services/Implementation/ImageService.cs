using DockerHubBackend.Models;
using DockerHubBackend.Repository.Implementation;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Services.Interface;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon;
using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.Runtime;
using DockerHubBackend.Config;
using Microsoft.Extensions.Options;

namespace DockerHubBackend.Services.Implementation
{
    public class ImageService : IImageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName = "uks-2024";    
     
        public ImageService(IOptions<AwsSettings> awsSettings)
        {
            var awsConfig = awsSettings.Value;

            // int AWS client
            var credentials = new BasicAWSCredentials(awsConfig.AccessKey, awsConfig.SecretKey);
            _s3Client = new AmazonS3Client(credentials, RegionEndpoint.GetBySystemName(awsConfig.Region));
        }

        // get image from s3
        public async Task<string> GetImageUrlAsync(string fileName)
        {
            try
            {
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = _bucketName,
                    Key = fileName,
                    Expires = DateTime.UtcNow.AddMinutes(60)
                };

                string preSignedUrl = _s3Client.GetPreSignedURL(request);
                return preSignedUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting image URL: {ex.Message}");
                return null;
            }
        }

        // upload file in s3
        public async Task UploadImageAsync(string estateName, Stream fileStream)
        {
            try
            {
                //bool folderExists = await DoesFolderExistAsync(_username);
                //if (!folderExists)
                //{
                //    await CreateFolderAsync(_username);
                //}

                var transferUtility = new TransferUtility(_s3Client);
                await transferUtility.UploadAsync(fileStream, _bucketName, estateName);
                Console.WriteLine("File uploaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading image: {ex.Message}");
            }
        }

        private async Task<bool> DoesFolderExistAsync(string folderName)
        {
            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = $"{folderName}/",  
                Delimiter = "/"
            };

            var response = await _s3Client.ListObjectsV2Async(request);
            return response.S3Objects.Count > 0;
        }

        private async Task CreateFolderAsync(string folderName)
        {
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = $"{folderName}/",  
                ContentBody = ""         
            };

            await _s3Client.PutObjectAsync(request);
            Console.WriteLine($"Folder {folderName} created.");
        }
    }
}
