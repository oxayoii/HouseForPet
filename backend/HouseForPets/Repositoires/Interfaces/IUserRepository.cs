using DataBaseContext.Enum;
using HouseForPet.DataBaseContext.Models.Pets;

public interface IUserRepository : IDisposable
{
    Task<int> AddAsync(User user);
    Task<bool> ExistsAsync(string login);
    Task<bool> FindLoginAsync(string login);
    Task<User> GetByLoginAsync(string login);
    Task<User> GetByRefreshTokenAsync(Guid refreshToken);
    Task<List<PermissionEnum>> GetPermissionsAsync(int userId);
    Task UpdateAsync(User user);
}