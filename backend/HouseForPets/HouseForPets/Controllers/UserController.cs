using Amazon.Runtime.Internal;
using Amazon.S3.Model;
using DataBaseContext.Dto.RequestModel;
using DataBaseContext.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.interfaces;
using Service.Services;
using System.Data;
using static Service.Middleware.CustomException;

namespace HouseForPets.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICaptchaService _captchaService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ICaptchaService captchaService, ILogger<UserController> logger)
        {
            _userService = userService;
            _captchaService = captchaService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Register(RequestUserRegister response)
        {
            var userId = await _userService.Register(response.Login, response.Password, response.RepeatPassword);
            return userId;
        }
        [HttpGet("new-captcha")]
        public async Task<IActionResult> GetNewCaptcha()
        {
            var captchaData = await _captchaService.GenerateCaptchaAsync();
       //   _logger.LogInformation("Captcha data retrieved: Question = {Question}, Token = {Token}", captchaData.Question, captchaData.Token);
            return Ok(captchaData);
        }
        [HttpPost("Auth")]
        public async Task<IResult> Login(RequestUserAuth response)
        {
            var tokens = await _userService.Login(response.Login, response.Password, response.CaptchaInput, response.CaptchaToken);

            return Results.Ok(tokens);
        }
        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh([FromQuery] string RefreshToken, string AccessToken)
        {
            var newTokens = await _userService.Refresh(RefreshToken, AccessToken);
            return Ok(newTokens);
        }
        [HttpGet]
        public async Task<ActionResult<HashSet<PermissionEnum>>> GetUserPermissions(string token)
        {
            var permissons = await _userService.CheckUserToken(token);
            return permissons;
        }
    }
}
