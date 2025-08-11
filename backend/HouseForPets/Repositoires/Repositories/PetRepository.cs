using DataBaseContext;
using DataBaseContext.Dto.RequestModel;
using HouseForPet.DataBaseContext.Models.Pets;
using HouseForPet.Service;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class PetRepository : IPetRepository
    {
        private readonly DataBasePrimaryContext _context;

        public PetRepository(DataBasePrimaryContext context)
        {
            _context = context;
        }

        public async Task<List<Pet>> GetAllPetsAsync(ResponsePetsSearch response)
        {
            return await _context.Pets.Filter(response).Sort(response).ToListAsync();
        }
        public async Task<Pet> GetPetByIdAsync(int id)
        {
            return await _context.Pets.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> CreatePetAsync(Pet pet)
        {
            await _context.Pets.AddAsync(pet);
            await _context.SaveChangesAsync();
            return pet.Id;
        }

        public async Task<bool> DeletePetAsync(int id)
        {
            var pet = await GetPetByIdAsync(id);
            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePetAsync(Pet pet)
        {
            _context.Pets.Update(pet);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
