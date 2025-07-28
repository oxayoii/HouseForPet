using DataBaseContext.Dto.ResponseModel;
using DataBaseContext.Models;
using HouseForPet.DataBaseContext.Models.Pets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Extensions;
using Service.interfaces;
using System.IdentityModel.Tokens.Jwt;
using static Service.Middleware.CustomException;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FavoritesController : Controller
    {
        private readonly IUserFavoriteService _favoriteService;
        public FavoritesController(IUserFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserFav()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var request = await _favoriteService.GetUserPets(token);
            return Ok(request);
        }
        [HttpPost]
        [Authorize]
        public async Task<int> AddFavPet([FromBody]int petId)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var request = await _favoriteService.AddFavPet(token, petId);
            return request;
        }
        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<ActionResult<bool>> DeleteFavPet(int id)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            bool request = await _favoriteService.DeleteFavPet(id, token);
            return Ok(request);
        }
    }
}
