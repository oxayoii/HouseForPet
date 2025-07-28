using HouseForPet.DataBaseContext.Models.Pets;

namespace Service.interfaces.AuthInterfaces
{
    public interface IJwtProvider
    {
        Guid GenerateRefreshToken();
        string GenerateAccessToken(User user);
    }
}