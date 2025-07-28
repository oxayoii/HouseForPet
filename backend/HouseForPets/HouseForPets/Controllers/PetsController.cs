using DataBaseContext;
using DataBaseContext.Dto.RequestModel;
using DataBaseContext.Enum;
using HouseForPet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.interfaces;
using Service.Services;

namespace HouseForPets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetsController : Controller
    {
        private readonly IPetsService _petsService;
        public PetsController(IPetsService petsService)
        {
            _petsService = petsService;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ResponsePetsSearch request)
        {
            var response = await _petsService.GetAllPets(request);
            return Ok(response);
        }
        [Authorize(Policy = "Create")]
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] RequestPets response)
        {
            var petId = await _petsService.CreatePet(response.ImageUrl, response.Name, response.Age.ToString(), PetsExtensions.ConvertToGender(response.Gender), response.Description);
            return Ok(petId);
        }
        [Authorize(Policy = "Delete")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            bool result = await _petsService.DeletePet(id);
            return Ok(result);
        }
        [Authorize(Policy = "Update")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<int>> Update(int id, [FromBody] RequestPets response)
        {
            bool result = await _petsService.UpdatePet(id, response.ImageUrl, response.Name, response.Age.ToString(), PetsExtensions.ConvertToGender(response.Gender), response.Description);
            return Ok(result);
        }
    }
}
