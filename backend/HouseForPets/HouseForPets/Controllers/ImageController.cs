using HouseForPet.DataBaseContext.Models.Pets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Service.Middleware.CustomException;

namespace HouseForPets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : Controller
    {
        private readonly IImageService _imageService;
        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }
       [Authorize (Policy = "Create")]
        [HttpPost]
        public async Task<string> CreateImage(IFormFile file)
        {
            var newImageKey = await _imageService.CreateImage(file);
            return newImageKey;
        }
        [Authorize(Policy = "Update")]
        [HttpPut]
        public async Task<ActionResult<string>> UpdateImage(IFormFile file, string key)
        {
            var newImageKey = await _imageService.UpdateImage(file, key);
            return Ok(newImageKey);
        }
        [Authorize(Policy = "Delete")]
        [HttpDelete]
        public async Task<ActionResult<string>> DeleteImage([FromQuery]string key) {
            bool result = await _imageService.DeleteImage(key);
            return Ok(result);
        }
    }
}
