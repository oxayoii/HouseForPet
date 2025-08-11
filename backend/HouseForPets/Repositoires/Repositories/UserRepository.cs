using DataBaseContext;
using DataBaseContext.Enum;
using HouseForPet.DataBaseContext.Models.Pets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories.Interfaces;

public class UserRepository : IUserRepository
{
    private readonly DataBasePrimaryContext _context;

    public UserRepository(DataBasePrimaryContext context)
    {
        _context = context;
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
        return user.Id;
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
    }

    public async Task<bool> ExistsAsync(string login)
    {
        return await _context.Users.AnyAsync(x => x.Login == login);
    }

    public async Task<List<PermissionEnum>> GetPermissionsAsync(int userId)
    {

        var permissions = await _context.UserPermissions
            .Where(x => x.UserId == userId)
            .Select(x => (PermissionEnum)x.PermissionId)
            .ToListAsync();

        return permissions;
    }

    private bool disposed = false;

    public virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
