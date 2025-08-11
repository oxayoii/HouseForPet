using DataBaseContext;
using DataBaseContext.Dto.ResponseModel;
using DataBaseContext.Enum;
using DataBaseContext.Models;
using HouseForPet.DataBaseContext.Models.Pets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Repositories.Repositories;
using Service.Extensions;
using Service.interfaces;
using Service.interfaces.AuthInterfaces;
using Service.Middleware;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Service.Middleware.CustomException;

namespace Service.Services
{
    public class UserService : IUserService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        private readonly UserExtensions _userExtensions;
        private readonly IRedisService _redisService;
        private readonly ICaptchaService _captchaService;
        private readonly IUserRepository _userRepository;
        public UserService(IPasswordHasher passwordHasher, IJwtProvider jwtProvider, 
            UserExtensions userExtensions, ICaptchaService captchaService, IRedisService redisService, IUserRepository userRepository)
        {
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _userExtensions = userExtensions;
            _captchaService = captchaService;
            _redisService = redisService;
            _userRepository = userRepository;
        }
        public async Task<int> Register(string Login, string Password, string repeatPassword)
        {
            if (Password != repeatPassword)
            {
                throw new BadRequestException("Пароли не совпадают");
            }
            if (Login.Length <= 4 || Login.Length >= 10 || Password.Length < 8)
            {
                throw new BadRequestException("Логин или пароль введены не корректно.");
            }
            if (!(Password.Any(char.IsUpper) && Password.Any(char.IsLower) && Password.Any(char.IsDigit)))
            {
                throw new BadRequestException("Пароль должен содержать как заглавные, так и строчные буквы, а также цифры.");
            }
            if (await _userRepository.FindLoginAsync(Login))
            {
                throw new BadRequestException("Пользователь с таким логином уже существует.");
            }

            var hashedPassword = _passwordHasher.Generate(Password);
            var newUser = new User(Login, hashedPassword);

            await _userRepository.AddAsync(newUser);

            return newUser.Id;
        }
        public async Task<TokenModelRequest> Login(string Login, string Password, string CaptchaInput, string CaptchaToken)
        {
            var requestKey = $"login_requests:{Login}";
            var blockKey = $"login_blocked:{Login}";

            if (_redisService.IsUserBlocked(blockKey))
            {
                throw new BadRequestException("Вы заблокированы на 5 минут за слишком много попыток входа. Попробуйте позже.");
            }
            if (!_redisService.IsRequestAllowed(requestKey, 5, TimeSpan.FromMinutes(1)))
            {
                _redisService.BlockUser(blockKey, TimeSpan.FromMinutes(5));
                throw new BadRequestException("Слишком много запросов. Вы заблокированы на 5 минут.");
            }

            bool isCaptchaValid = await _captchaService.ValidateCaptchaAsync(CaptchaToken, CaptchaInput);
            if (!isCaptchaValid)
            {
                throw new BadRequestException("Неккоректная каптча");
            }

            var user = await _userRepository.GetByLoginAsync(Login);
            if (user == null)
            {
                throw new NotFoundException("Пользователя не существует.");
            }

            var result = _passwordHasher.Verify(Password, user.PasswordHash);
            if (!result)
            {
                throw new BadRequestException("Неверный пароль.");
            }
            var refreshToken = _jwtProvider.GenerateRefreshToken();
            var accessToken = _jwtProvider.GenerateAccessToken(user);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30);

            await _userRepository.UpdateAsync(user);

            return new TokenModelRequest
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.ToString()
            };
        }
        public async Task<TokenModelRequest> Refresh(string refreshToken, string accessToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new NotFoundException("Refresh token is required");
            }
            if (!Guid.TryParse(refreshToken, out Guid refreshTokenGuid))
            {
                throw new BadRequestException("Invalid refresh token format.");
            }

            var user = await _userRepository.GetByRefreshTokenAsync(refreshTokenGuid);

            if (user == null)
            {
                throw new BadRequestException("Invalid refresh token");
            }

            Guid refreshToken2;
            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new UnauthorizedException("Войдите в систему заново");
            }
            else
            {
                refreshToken2 = user.RefreshToken;
            }

            var newAccessToken = "";

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(accessToken);
            var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp");
            if (expClaim != null)
            {
                var expDateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim.Value)).UtcDateTime;
                if (expDateTime <= DateTime.UtcNow)
                {
                    newAccessToken = _jwtProvider.GenerateAccessToken(user);
                }
                else
                {
                    newAccessToken = accessToken;
                }
            }

            return new TokenModelRequest
            {
                AccessToken = newAccessToken,
                RefreshToken = refreshToken2.ToString()
            };
        }
        public async Task<HashSet<PermissionEnum>> CheckUserToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new BadRequestException("Токен не может быть пустым.");
            }

            var principal = _userExtensions.GetPrincipalFromExpiredToken(token);
            if (principal == null)
            {
                throw new BadRequestException("Некорректный токен.");
            }

            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                throw new BadRequestException("Некорректный идентификатор пользователя.");
            }

            if (userId <= 0)
            {
                throw new BadRequestException("Некорректный идентификатор пользователя.");
            }

            var permissions = await GetUserPermission(userId);
            return permissions;
        }

        public async Task<HashSet<PermissionEnum>> GetUserPermission(int userId)
        {
            var userPermissions = await _userRepository.GetPermissionsAsync(userId);

            return userPermissions.ToHashSet();
        }
    }
}
