using DataBaseContext.Dto.ResponseModel;
using DataBaseContext.Enum;

namespace Service.interfaces
{
    public interface IUserService
    {
        Task<int> Register(string Login, string Password, string repeatPassword);
        Task<TokenModelRequest> Login(string Login, string Password, string CaptchaInput, string CapthaToken);
        Task<HashSet<PermissionEnum>> GetUserPermission(int userId);
        Task<HashSet<PermissionEnum>> CheckUserToken(string token);
        Task<TokenModelRequest> Refresh(string refreshToken, string accessToken);
    }
}