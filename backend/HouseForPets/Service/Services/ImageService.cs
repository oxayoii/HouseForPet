using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using Service.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Service.Middleware.CustomException;

namespace Service.Services
{
    public class ImageService : IImageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly string _endpount;
        public ImageService(IAmazonS3 s3Client, IConfiguration configuration)
        {
            _s3Client = s3Client;
            _bucketName = configuration["YandexCloud:BucketName"];
            _endpount = configuration["YandexCloud:Endpoint"];
        }
        public async Task<string> GetImage(string key)
        {
            var baseUrl = $"{_endpount}/{_bucketName}/{key}";
            return baseUrl;
        }
        public async Task<string> CreateImage(IFormFile file)
        {
            if (file == null)
            {
                throw new BadRequestException("Файл не был загружен.");
            }
            var key = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            
            using var sha256 = SHA256.Create();
            var hash = BitConverter.ToString(sha256.ComputeHash(stream)).Replace("-", "").ToLower();

            stream.Position = 0;
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = stream,
                AutoCloseStream = false,
                ContentType = file.ContentType,
                ChecksumSHA256 = hash
            };
            await _s3Client.PutObjectAsync(request);
            return key;
        }
        public async Task<string> UpdateImage(IFormFile file, string id)
        {
            if (file == null || file.Length == 0)
            {
                throw new BadRequestException("Новый файл не был загружен.");
            }
            await _s3Client.DeleteObjectAsync(_bucketName, id);

            var newKey = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            try
            {
                await _s3Client.GetObjectMetadataAsync(_bucketName, id);
            }
            catch (AmazonS3Exception ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new NotFoundException($"Изображение с key '{id}' не найдено в хранилище.");
                }
            }
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            using var sha256 = SHA256.Create();
            var hash = BitConverter.ToString(sha256.ComputeHash(stream)).Replace("-", "").ToLower();

            stream.Position = 0;
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = newKey,
                InputStream = stream,
                AutoCloseStream = false,
                ContentType = file.ContentType,
                ChecksumSHA256 = hash
            };
            await _s3Client.PutObjectAsync(request);
            return newKey;
        }
        public async Task<bool> DeleteImage(string key)
        {
            await _s3Client.DeleteObjectAsync(_bucketName, key);
            return true;
        }
    }
}
