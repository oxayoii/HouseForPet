using DataBaseContext;
using HouseForPet.DataBaseContext.Models.Pets;
using Microsoft.EntityFrameworkCore;
using Service.interfaces;
using static Service.Middleware.CustomException;
using DataBaseContext.Dto.ResponseModel;
using DataBaseContext.Dto.RequestModel;
using Newtonsoft.Json;
using Repositories.Interfaces;

namespace Service.Services
{
    public class PetsService : IPetsService
    {
        private readonly IImageService _imageService;
        private readonly IRedisPets _redisPets;
        private readonly IUnitOfWork _unitOfWork;
        public PetsService(IImageService imageService, IRedisPets redisPets, IUnitOfWork unitOfWork)
        {
            _imageService = imageService;
            _redisPets = redisPets;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponsePets> GetAllPets(ResponsePetsSearch response)
        {
            string cacheKey = $"pets1:{JsonConvert.SerializeObject(response)}";  
            List<PetsDTO> petsDto = await _redisPets.GetPetsFromCacheAsync(cacheKey);

            if (petsDto != null)
            {
                return new ResponsePets(petsDto);
            }

            var query = await _unitOfWork.pets.GetAllPetsAsync(response);
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
            try
            {
                if (string.IsNullOrWhiteSpace(Name) || Name.Length < 3 || Name.Length > 15)
                {
                    throw new BadRequestException("Длина имени питомца должна составлять от 2 до 15 символов.");
                }
                if (!int.TryParse(Age, out int AgeInt) || AgeInt <= 0 || AgeInt > 15)
                {
                    throw new BadRequestException("Возраст питомца должен быть от 0 до 15.");
                }
                if (imageKey.Length == 0)
                {
                    throw new BadRequestException("Выберите изображение.");
                }
                if (string.IsNullOrWhiteSpace(Description) || Description.Length < 10 || Description.Length > 200)
                {
                    throw new BadRequestException("Длина описания должна составлять от 10 до 200 символов.");
                }
                await _unitOfWork.BeginTransactionAsync();
                var pet = new Pet(imageKey, Name, AgeInt, Gender, Description);
                int petId = await _unitOfWork.pets.CreatePetAsync(pet);
                await _unitOfWork.CommitAsync();
                await _redisPets.InvalidateAllPetsCache();
                return petId;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        public async Task<bool> DeletePet(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var pet = await _unitOfWork.pets.GetPetByIdAsync(id);
                if (pet == null)
                {
                    throw new NotFoundException("Питомец не найден.");
                }
                await _unitOfWork.pets.DeletePetAsync(id);
                await _unitOfWork.CommitAsync();
                await _redisPets.InvalidateAllPetsCache();
                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        public async Task<bool> UpdatePet(int Id, string ImageUrl, string Name, string Age, Sex Gender, string Description)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var pet = await _unitOfWork.pets.GetPetByIdAsync(Id);
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
                bool result = await _unitOfWork.pets.UpdatePetAsync(pet);
                await _unitOfWork.CommitAsync();
                await _redisPets.InvalidateAllPetsCache();

                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
