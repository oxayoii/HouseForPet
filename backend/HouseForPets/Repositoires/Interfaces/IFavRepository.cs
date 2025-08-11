using DataBaseContext.Models;

namespace Repositories.Interfaces
{
    public interface IFavRepository
    {
        Task<int> AddAsync(UserFavorite favorite);
        Task<bool> ExistsAsync(int id);
        Task<UserFavorite> GetFavoriteByIdAsync(int id);
        Task<UserFavorite> GetFavoriteByUserAndPetAsync(int userId, int petId);
        Task<List<UserFavorite>> GetUserFavoritesAsync(int userId);
        Task<bool> Remove(int id);
    }
}