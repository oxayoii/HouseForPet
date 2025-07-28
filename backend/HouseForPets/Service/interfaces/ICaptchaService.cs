using DataBaseContext.Dto.ResponseModel;

namespace Service.interfaces
{
    public interface ICaptchaService
    {
        Task<ResponseCaptcha> GenerateCaptchaAsync();
        Task<bool> ValidateCaptchaAsync(string token, string userInput);
    }
}