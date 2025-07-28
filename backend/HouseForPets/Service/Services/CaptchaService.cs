using Amazon.Runtime.Internal.Util;
using DataBaseContext.Dto.RequestModel;
using DataBaseContext.Dto.ResponseModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service.Services
{
    public class CaptchaService : ICaptchaService
    {
        private const int TokenExpirationSeconds = 60;
        private readonly ILogger<CaptchaService> _logger;
        public CaptchaService(ILogger<CaptchaService> logger)
        {
            _logger = logger;
        }
        public async Task<ResponseCaptcha> GenerateCaptchaAsync()
        {
            // 1. Генерируем математический пример.
            var captchaData = GenerateMathCaptcha();

            // 2. Создаем токен капчи.
            string token = GenerateCaptchaToken(captchaData.Answer.ToString());

            _logger.LogInformation("Captcha generated: Question = {Question}, Token = {Token}", captchaData.Question, token);
            // 3. Возвращаем пример и токен.
            return ( new ResponseCaptcha
            {
                Question = captchaData.Question,
                Token = token
            });
        }

        public Task<bool> ValidateCaptchaAsync(string token, string userInput)
        {
            // 1. Проверка токена капчи.
            bool isValid = ValidateCaptchaToken(token, userInput);

            return Task.FromResult(isValid);
        }

        private (string Question, int Answer) GenerateMathCaptcha()
        {
            Random rnd = new Random();
            int num1 = rnd.Next(1, 10);
            int num2 = rnd.Next(1, 10);
            int operation = rnd.Next(0, 2); // 0: +, 1: -

            string question;
            int answer;

            if (operation == 0)
            {
                question = $"{num1} + {num2} = ?";
                answer = num1 + num2;
            }
            else
            {
                question = $"{num1} - {num2} = ?";
                answer = num1 - num2;
            }
            
            return (question, answer);
        }

        private string GenerateCaptchaToken(string captchaAnswer)
        {
            // Создаем timestamp, который будет частью токена
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // Комбинируем текст капчи и timestamp
            string combinedString = $"{captchaAnswer}:{timestamp}";

            // Создаем SHA256 хеш
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(combinedString));

                // Конвертируем byte array в string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2")); // hex representation
                }
                return builder.ToString();
            }
        }


        private bool ValidateCaptchaToken(string token, string userInput)
        {
            // 1. Извлекаем timestamp из токена (поскольку у нас его там нет, нам нужно его воссоздать)
            //   Мы не храним timestamp в самом токене, поэтому нужно проверить все возможные значения за последние X секунд.
            long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            for (long timestamp = currentTimestamp; timestamp > currentTimestamp - TokenExpirationSeconds; timestamp--)
            {
                // 2. Генерируем ожидаемый токен, используя userInput и timestamp
                string combinedString = $"{userInput}:{timestamp}";

                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(combinedString));
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    string expectedToken = builder.ToString();

                    // 3. Сравниваем ожидаемый токен с полученным
                    if (expectedToken == token)
                    {
                        return true; // Токен валиден
                    }
                }
            }

            return false; // Токен невалиден или устарел
        }
    }
}
