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
        private readonly string _username = "examplegmail.com";   
     
        public ImageService(IOptions<AwsSettings> awsSettings)
        {
            var awsConfig = awsSettings.Value;

            // Inicijalizacija AWS S3 klijenta
            var credentials = new BasicAWSCredentials(awsConfig.AccessKey, awsConfig.SecretKey);
            _s3Client = new AmazonS3Client(credentials, RegionEndpoint.GetBySystemName(awsConfig.Region)); // Region "eu-central-1"
        }

        // Metod za generisanje URL-a slike sa S3
        public async Task<string> GetImageUrlAsync(string fileName)
        {
            try
            {
                // Proveravamo da li je u imenu fajla & - treba da podelimo ime i folder
                if (fileName.Contains("&"))
                {
                    var tokens = fileName.Split('&');
                    var folder = tokens[0];
                    var file = tokens[1];
                    fileName = $"{folder}/{file}";
                }

                // Sastavljamo URL
                var request = new GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileName
                };

                var response = await _s3Client.GetObjectAsync(request);
                var url = $"https://{_bucketName}.s3.{RegionEndpoint.EUCentral1.SystemName}.amazonaws.com/{fileName}";
                return url;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting image URL: {ex.Message}");
                return null;
            }
        }

        // Metod za upload fajla na S3
        public async Task UploadImageAsync(string estateName, Stream fileStream)
        {
            try
            {
                // Provera da li folder za korisnika postoji, ako ne, kreiraj ga
                bool folderExists = await DoesFolderExistAsync(_username);
                if (!folderExists)
                {
                    await CreateFolderAsync(_username);
                }

                if (estateName.Contains("&"))
                {
                    var tokens = estateName.Split('&');
                    var folder = tokens[0];
                    var file = tokens[1];
                    estateName = $"{folder}/{file}";
                }

                var transferUtility = new TransferUtility(_s3Client);
                await transferUtility.UploadAsync(fileStream, _bucketName, estateName);
                Console.WriteLine("File uploaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading image: {ex.Message}");
            }
        }

        // Proverava da li postoji folder u S3
        private async Task<bool> DoesFolderExistAsync(string folderName)
        {
            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = $"{folderName}/",  // S3 koristi prefixe za simulaciju foldera
                Delimiter = "/"
            };

            var response = await _s3Client.ListObjectsV2Async(request);
            return response.S3Objects.Count > 0;
        }

        // Kreira folder u S3 (simulacija foldera)
        private async Task CreateFolderAsync(string folderName)
        {
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = $"{folderName}/",  // Ime foldera sa "/"
                ContentBody = ""         // Prazan sadržaj
            };

            await _s3Client.PutObjectAsync(request);
            Console.WriteLine($"Folder {folderName} created.");
        }
    }
}
