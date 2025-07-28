using Microsoft.AspNetCore.Http;

namespace Service.interfaces
{
    public interface IImageService
    {
        Task<string> GetImage(string key);
        Task<string> CreateImage(IFormFile imageStream);
        Task<string> UpdateImage(IFormFile file, string key);
        Task<bool> DeleteImage(string key);
    }
}