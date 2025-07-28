using DataBaseContext;
using HouseForPet.DataBaseContext.Models.Pets;
using HouseForPet.Service;
using Microsoft.EntityFrameworkCore;
using Service.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Service.Middleware.CustomException;
using static System.Net.Mime.MediaTypeNames;
using Service.Extensions;
using DataBaseContext.Dto.ResponseModel;
using DataBaseContext.Dto.RequestModel;
using Newtonsoft.Json;

namespace Service.Services
{
    public class PetsService : IPetsService
    {
        private readonly DataBasePrimaryContext _context;
        private readonly IImageService _imageService;
        private readonly IRedisPets _redisPets;
        public PetsService(DataBasePrimaryContext context, IImageService imageService, IRedisPets redisPets)
        {
            _context = context;
            _imageService = imageService;
            _redisPets = redisPets;
        }
        public async Task<ResponsePets> GetAllPets(ResponsePetsSearch response)
        {
            string cacheKey = $"pets1:{JsonConvert.SerializeObject(response)}";  
            List<PetsDTO> petsDto = await _redisPets.GetPetsFromCacheAsync(cacheKey);

            if (petsDto != null)
            {
                return new ResponsePets(petsDto);
            }

            var query = await _context.Pets.Filter(response).Sort(response).ToListAsync();
            petsDto = new List<PetsDTO>();

            foreach (var x in query)
            {
                var imageUrl = await _imageService.GetImage(x.ImageKey);
                petsDto.Add(new PetsDTO
                {
                    Id = x.Id,
                    ImageUrl = imageUrl,
                    Name = x.Name,
                    Age = x.Age,
                    Gender = x.Gender.ToString(),
                    Description = x.Description
                });
            }
            await _redisPets.SetPetsInCacheAsync(cacheKey, petsDto);
            return new ResponsePets(petsDto);
        }
        public async Task<int> CreatePet(string imageKey, string Name, string Age, Sex Gender, string Description)
        {
            if (string.IsNullOrWhiteSpace(Name) || Name.Length < 3 || Name.Length > 15)
            {
                throw new BadRequestException("Длина имени питомца должна составлять от 2 до 15 символов.");
            }
            if (!int.TryParse(Age, out int AgeInt) || AgeInt <= 0 || AgeInt > 15)
            {
                throw new BadRequestException("Возраст питомца должен быть от 0 до 15.");
            }
            if (imageKey.Length == 0) {
                throw new BadRequestException("Выберите изображение.");
            }
            if (string.IsNullOrWhiteSpace(Description) || Description.Length < 10 || Description.Length > 200)
            {
                throw new BadRequestException("Длина описания должна составлять от 10 до 200 символов.");
            }
            var pet = new Pet(imageKey, Name, AgeInt, Gender, Description);
            await _context.Pets.AddAsync(pet);
            await _context.SaveChangesAsync();
            await _redisPets.InvalidateAllPetsCache();
            return pet.Id;
        }
        public async Task<bool> DeletePet(int id)
        {
            var pet = await _context.Pets.FirstOrDefaultAsync(x => x.Id == id);
            if (pet == null)
            {
                throw new NotFoundException("Питомец не найден.");
            }
            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
            await _redisPets.InvalidateAllPetsCache();
            return true;
        }
        public async Task<bool> UpdatePet(int Id, string ImageUrl, string Name, string Age, Sex Gender, string Description)
        {
            var pet = await _context.Pets.FirstOrDefaultAsync(x => x.Id == Id);
            if (pet == null)
            {
                throw new NotFoundException("Питомец не найден.");
            }
            if (string.IsNullOrWhiteSpace(Name) || Name.Length < 3 || Name.Length > 10)
            {
                throw new BadRequestException("Длина имени питомца должна составлять от 2 до 10 символов.");
            }
            if (!int.TryParse(Age, out int AgeInt) || AgeInt <= 0 || AgeInt > 15)
            {
                throw new BadRequestException("Возраст питомца должен быть от 0 до 15.");
            }
            if (ImageUrl.Length < 1)
            {
                throw new BadRequestException("Выберите изображение.");
            }
            if (Description.Length <= 10 && Description.Length >= 100)
            {
                throw new BadRequestException("Длина описания должна составлять от 10 до 100 символов.");
            }
            if (!Enum.IsDefined(typeof(Sex), Gender))
            {
                throw new BadRequestException("Недопустимое значение для пола питомца.");
            }
            pet.ImageKey = ImageUrl;
            pet.ModifyDate = DateTime.UtcNow;
            pet.Name = Name;
            pet.Age = AgeInt;
            pet.Gender = Gender;
            pet.Description = Description;
            _context.Pets.Update(pet);
            await _context.SaveChangesAsync();
            await _redisPets.InvalidateAllPetsCache();

            return true;
        }
    }
}
