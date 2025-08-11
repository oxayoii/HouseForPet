using Amazon.Auth.AccessControlPolicy;
using DataBaseContext;
using DataBaseContext.Dto.ResponseModel;
using DataBaseContext.Models;
using HouseForPet.DataBaseContext.Models.Pets;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Service.Extensions;
using Service.interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Service.Middleware.CustomException;

namespace Service.Services
{
    public class UserFavoriteService : IUserFavoriteService
    {
        private readonly UserExtensions _userExtensions;
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _unitOfWork;
        public UserFavoriteService(IUnitOfWork unitOfWork, IImageService imageService, UserExtensions userExtensions)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _userExtensions = userExtensions;
        }
        public async Task<ResponseFavPets> GetUserPets(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new BadRequestException("Токен не может быть пустым.");

            var principal = _userExtensions.GetPrincipalFromExpiredToken(token);
            if (principal == null)
                throw new BadRequestException("Неккоректный токен");

            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                throw new BadRequestException("Неккоректный индефикатор пользователя.");

            var userFavorites = await _unitOfWork.favs.GetUserFavoritesAsync(userId);
            var favDto = new List<FavDto>();

            foreach (var uf in userFavorites)
            {
                var pet = uf.Pet;
                if (pet != null)
                {
                    var imageUrl = await _imageService.GetImage(pet.ImageKey);
                    favDto.Add(new FavDto
                    {
                        Id = uf.Id,
                        PetId = pet.Id,
                        ImageUrl = imageUrl,
                        Name = pet.Name,
                        Age = pet.Age,
                        Gender = pet.Gender.ToString(),
                        Description = pet.Description
                    });
                }
            }
            return new ResponseFavPets(favDto);
        }

        public async Task<int> AddFavPet(string token, int PetId)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new BadRequestException("Токен не может быть пустым.");
            }
            var principal = _userExtensions.GetPrincipalFromExpiredToken(token);
            if (principal == null)
            {
                throw new BadRequestException("Неккоректный токен");
            }
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                throw new BadRequestException("Неккоректный индефикатор пользователя.");
            }
            if (PetId == 0)
            {
                throw new NotFoundException("Выберите питомца.");
            }
            var petExists = await _unitOfWork.favs.ExistsAsync(PetId);
            if (!petExists)
            {
                throw new NotFoundException("Питомец с таким ID не найден.");
            }
            var existingFavPet = await _unitOfWork.favs.GetFavoriteByUserAndPetAsync(userId, PetId); 

            if (existingFavPet != null)
            {
                throw new BadRequestException("Этот питомец уже добавлен в избранное.");
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var favPet = new UserFavorite(userId, PetId);
                await _unitOfWork.favs.AddAsync(favPet);
                await _unitOfWork.CommitAsync();
                return favPet.Id;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        public async Task<bool> DeleteFavPet(int id, string token)
        {
            var principal = _userExtensions.GetPrincipalFromExpiredToken(token);
            if (principal == null)
            {
                throw new BadRequestException("Неккоректный токен");
            }
            if (id <= 0)
            {
                throw new BadRequestException("Некорректный идентификатор питомца.");
            }
            var pet = await _unitOfWork.favs.GetFavoriteByIdAsync(id);
            if (pet == null)
            {
                throw new NotFoundException("Питомец не найден.");
            }
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                throw new BadRequestException("Неккоректный индефикатор пользователя.");
            }
            if (pet.UserId != userId)
            {
                throw new UnauthorizedAccessException("У вас нет прав на удаление этого питомца.");
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var result = await _unitOfWork.favs.Remove(id);
                await _unitOfWork.CommitAsync();
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
