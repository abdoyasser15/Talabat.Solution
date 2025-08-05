using Microsoft.AspNetCore.Identity;
using Talbat.Core.Entities.Identity;
using Talbat.Core.Services.Contract;
using Talbat.Repository.Identity;
using Talbat.Service;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace Talabat.APIs.Extensions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddScoped<IAuthServices, AuthService>();

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                //options.Password.RequiredUniqueChars = 2;// Password must have at least 2 unique characters
                //options.Password.RequireLowercase = true; // Password must have at least 1 lowercase character
                //options.Password.RequireUppercase = true; // Password must have at least 1 uppercase character
                //options.Password.RequireNonAlphanumeric = true; // Password must have at least 1 non-alphanumeric character
            }).AddEntityFrameworkStores<AppIdentityDbContext>();
            // Register Identity Services (signIn Manager, UserManager, RoleManager)

            services.AddAuthentication(options=>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Default Authentication Scheme
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Default Challenge Scheme
            }) // Use Bearer Authentication Scheme
                .AddJwtBearer("Bearer" ,options =>
                {
                    // Configure Authentication Handler
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidAudience = configuration["JWT:Audience"],
                        ValidateIssuer = true,
                        ValidIssuer = configuration["JWT:Issuer"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SeceretKey"])),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromDays(double.Parse(configuration["JWT:DurationInDays"]))
                    };
                });
            return services;
        }
    }
}
