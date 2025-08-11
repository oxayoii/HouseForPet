using DataBaseContext;
using DataBaseContext.Enum;
using HouseForPet.DataBaseContext.Models.Pets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories.Interfaces;

public class UserRepository : IUserRepository
{
    private readonly DataBasePrimaryContext _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(DataBasePrimaryContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<User> GetByLoginAsync(string login)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Login == login);
    }

    public async Task<bool> FindLoginAsync(string login)
    {
        return await _context.Users.AnyAsync(x => x.Login == login);
    }

    public async Task<User> GetByRefreshTokenAsync(Guid refreshToken)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
    }

    public async Task<int> AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user.Id;
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(string login)
    {
        return await _context.Users.AnyAsync(x => x.Login == login);
    }

    public async Task<List<PermissionEnum>> GetPermissionsAsync(int userId)
    {
        _logger.LogInformation("Запрос прав пользователя с ID: {User Id}", userId);

        var permissions = await _context.UserPermissions
            .Where(x => x.UserId == userId)
            .Select(x => (PermissionEnum)x.PermissionId)
            .ToListAsync();

        _logger.LogInformation("Права пользователя с ID: {User Id} получены: {PermissionsCount} прав(а)", userId, permissions.Count);

        return permissions;
    }
}
