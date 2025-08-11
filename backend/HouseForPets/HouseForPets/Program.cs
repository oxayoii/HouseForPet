using Amazon.S3;
using DataBaseContext;
using DataBaseContext.Enum;
using DataBaseContext.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repositories;
using Repositories.Interfaces;
using Repositories.Repositories;
using Service;
using Service.Auth;
using Service.Extensions;
using Service.interfaces;
using Service.interfaces.AuthInterfaces;
using Service.Middleware;
using Service.Services;
using Service.Services.Auth;
using Service.Services.Redis;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;

namespace HouseForPets
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
            builder.Services.AddApiAuthentication(builder.Services.BuildServiceProvider().GetService<IOptions<JwtOptions>>());
            builder.Services.AddHttpContextAccessor();


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "??????? ????? ? ?????? ? ??????? Basic",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                { new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "basic"
                        }
                    },
                    new string[] { }
                }
            });
            });

            builder.Services.AddScoped<IPetsService, PetsService>();
            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IJwtProvider, JwtProvider>();
            builder.Services.AddScoped<IUserFavoriteService, UserFavoriteService>();
            builder.Services.AddScoped<UserExtensions>();
            builder.Services.AddScoped<ICaptchaService, CaptchaService>();
            builder.Services.AddScoped<IRedisService, RedisService>();
            builder.Services.AddScoped<IRedisPets, RedisPets>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IPetRepository, PetRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IFavRepository, FavRepository>();

            builder.Services.AddSingleton<IConnectionMultiplexer>(provider =>
            {
                var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"), true);
                configuration.AbortOnConnectFail = false;
                return ConnectionMultiplexer.Connect(configuration);
            });

            builder.Services.AddDbContext<DataBasePrimaryContext>(
       options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IAmazonS3>(sp =>
            {
                var config = new AmazonS3Config
                {
                    ServiceURL = builder.Configuration["YandexCloud:Endpoint"],
                    ForcePathStyle = true
                };

                return new AmazonS3Client(
                    builder.Configuration["YandexCloud:AccessKey"],
                    builder.Configuration["YandexCloud:SecretKey"],
                    config);
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:5173")
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowSpecificOrigin");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.MapControllers();

            app.Run();
        }
    }
}
