using DataBaseContext.Enum;
using DataBaseContext.Models;
using HouseForPet.DataBaseContext.Models.Pets;

namespace Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<int> AddAsync(User user);
        Task<bool> ExistsAsync(string login);
        Task<User> GetByLoginAsync(string login);
        Task<User> GetByRefreshTokenAsync(Guid refreshToken);
        Task<List<PermissionEnum>> GetPermissionsAsync(int userId);
        Task UpdateAsync(User user);
        Task<bool> FindLoginAsync(string login);
    }
}