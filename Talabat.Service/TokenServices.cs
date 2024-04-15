using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.Service
{
    public class TokenServices : ITokenServices
    {
        private readonly IConfiguration Configuration;

        public TokenServices(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser User,UserManager<AppUser> userManager)
        {
            //Payload
            //PrivateClaims
            var AuthClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,User.DisplayName),
                new Claim(ClaimTypes.Email,User.Email),
            };

            var UsersRole = await userManager.GetRolesAsync(User);
            foreach (var Role in UsersRole)
            {
                AuthClaims.Add(new Claim(ClaimTypes.Role, Role));
            }
            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]));

            var Token = new JwtSecurityToken
                (
                issuer: Configuration["JWT:ValidIssuer"],
                audience: Configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(double.Parse(Configuration["JWT:DurationInDays"])),
                claims:AuthClaims,
                signingCredentials:new SigningCredentials(AuthKey,SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
