using DataBaseContext.Dto.RequestModel;
using DataBaseContext.Dto.ResponseModel;
using HouseForPet.DataBaseContext.Models.Pets;

namespace Service.interfaces
{
    public interface IPetsService
    {
        Task<DataBaseContext.Dto.ResponseModel.ResponsePets> GetAllPets(ResponsePetsSearch response);
        Task<int> CreatePet(string imageKey, string Name, string Age, Sex Gender, string Description);
        Task<bool> DeletePet(int id);
        Task<bool> UpdatePet(int Id, string ImageKey, string Name, string Age, Sex Gender, string Description);
    }
}