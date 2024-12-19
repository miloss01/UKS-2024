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
        public async Task<string> GetImageUrl(string fileName)
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
        public async Task UploadImage(string imageName, Stream fileStream)
        {
            try
            {
                var transferUtility = new TransferUtility(_s3Client);
                await transferUtility.UploadAsync(fileStream, _bucketName, imageName);
                Console.WriteLine("File uploaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading image: {ex.Message}");
            }
        }

        // delete image from S3
        public async Task DeleteImage(string filePath)
        {
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = filePath
                };

                await _s3Client.DeleteObjectAsync(deleteObjectRequest);
                Console.WriteLine($"File '{filePath}' deleted successfully.");
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Amazon S3 error while deleting file: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file: {ex.Message}");
            }
        }

    }
}
