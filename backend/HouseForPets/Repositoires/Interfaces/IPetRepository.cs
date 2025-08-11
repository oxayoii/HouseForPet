using DataBaseContext.Dto.RequestModel;
using HouseForPet.DataBaseContext.Models.Pets;

namespace Repositories.Interfaces
{
    public interface IPetRepository
    {
        Task<int> CreatePetAsync(Pet pet);
        Task<bool> DeletePetAsync(int id);
        Task<List<Pet>> GetAllPetsAsync(ResponsePetsSearch response);
        Task<Pet> GetPetByIdAsync(int id);
        Task<bool> UpdatePetAsync(Pet pet);
    }
}