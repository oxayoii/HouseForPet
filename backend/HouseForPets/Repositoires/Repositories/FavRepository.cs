using DataBaseContext;
using DataBaseContext.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class FavRepository : IFavRepository
    {
        private readonly DataBasePrimaryContext _context;

        public FavRepository(DataBasePrimaryContext context)
        {
            _context = context;
        }

        public async Task<List<UserFavorite>> GetUserFavoritesAsync(int userId)
        {
            return await _context.UserFavorites
                .Where(uf => uf.UserId == userId)
                .Include(uf => uf.Pet)
                .ToListAsync();
        }

        public async Task<UserFavorite> GetFavoriteByIdAsync(int id)
        {
            return await _context.UserFavorites
                .Include(uf => uf.Pet)
                .FirstOrDefaultAsync(uf => uf.Id == id);
        }

        public async Task<UserFavorite> GetFavoriteByUserAndPetAsync(int userId, int petId)
        {
            return await _context.UserFavorites
                .FirstOrDefaultAsync(uf => uf.UserId == userId && uf.PetId == petId);
        }

        public async Task<int> AddAsync(UserFavorite favorite)
        {
            await _context.UserFavorites.AddAsync(favorite);
            return favorite.Id;
        }

        public async Task<bool> Remove(int id)
        {
            var pet = await GetFavoriteByIdAsync(id);
            _context.UserFavorites.Remove(pet);
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.UserFavorites.AnyAsync(uf => uf.Id == id);
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
}
