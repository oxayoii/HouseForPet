using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;


using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using Service.Auth;
using DataBaseContext.Enum;
using Service.Services.Auth;
using Microsoft.AspNetCore.Authorization;

namespace Service
{
    public static class ApiExtensions
    {
        public static void AddApiAuthentication(this IServiceCollection services, IOptions<JwtOptions> jwtOptions)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.SecretKey))
                    };
                });
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Create", policy =>
                    policy.Requirements.Add(new PermissionRequirement([PermissionEnum.Create])));
                options.AddPolicy("Update", policy =>
                    policy.Requirements.Add(new PermissionRequirement([PermissionEnum.Update])));
                options.AddPolicy("Delete", policy =>
                    policy.Requirements.Add(new PermissionRequirement([PermissionEnum.Delete])));
            });
        }
    }
}
