using DataBaseContext.Dto.ResponseModel;

namespace Service.interfaces
{
    public interface IRedisPets
    {
        Task<List<PetsDTO>> GetPetsFromCacheAsync(string cacheKey);
        Task SetPetsInCacheAsync(string cacheKey, List<PetsDTO> pets);
        Task InvalidateAllPetsCache();
    }
}