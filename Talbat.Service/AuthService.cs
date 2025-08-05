using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Entities.Identity;
using Talbat.Core.Services.Contract;

namespace Talbat.Service
{
    public class AuthService : IAuthServices
    {
        private readonly IConfiguration configuration;

        public AuthService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<string> CreateTokenAsunc(AppUser User, UserManager<AppUser> userManager)
        {
            // Private Claims (User-Definded)
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,User.UserName),
                new Claim(ClaimTypes.Email,User.Email) 
            };
            var UserRoles = await userManager.GetRolesAsync(User);
            foreach (var role in UserRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SeceretKey"]));

            var token  = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddDays(double.Parse(configuration["JWT:DurationInDays"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
