using DataBaseContext.Dto.ResponseModel;
using DataBaseContext.Models;
using HouseForPet.DataBaseContext.Models.Pets;

namespace Service.interfaces
{
    public interface IUserFavoriteService
    {
        Task<ResponseFavPets> GetUserPets(string token);
        Task<int> AddFavPet(string token, int PetId);
        Task<bool> DeleteFavPet(int id, string token);
    }
}