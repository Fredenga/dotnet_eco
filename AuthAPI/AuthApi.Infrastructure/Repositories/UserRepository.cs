using AuthApi.Application.DTOs;
using AuthApi.Application.Interfaces;
using AuthApi.Domain.Entities;
using AuthApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedLib.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthApi.Infrastructure.Repositories
{
    public class UserRepository(AuthDbContext context, IConfiguration config) : IUser
    {
        private async Task<AppUser> GetUserByEmail(string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == email);
            return user is not null ? user : null!;
        }
        public async Task<GetUserDTO> GetUser(int userID)
        {
            var user = await context.Users.FindAsync(userID);
            return user is not null ? new GetUserDTO(
                user.ID,
                user.Name!,
                user.Telephone!,
                user.Address!,
                user.Email!,
                user.Role!
                ) : null!;
        }

        public async Task<Response> Login(LoginDTO loginDTO)
        {
            var user = await GetUserByEmail(loginDTO.email);
            if (user is null) {
                return new Response(false, "invalid email provided");
            }
            //verify password
            bool verified = BCrypt.Net.BCrypt.Verify(loginDTO.password, user!.Password);
            if (!verified) {
                return new Response(false, "invalid password provided");
            }

            string token = GenerateToken(user);
            return new Response(true, token);
        }

        private string GenerateToken(AppUser user)
        {
            var key = Encoding.UTF8.GetBytes(config.GetSection("Authentication:Key").Value!);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new (ClaimTypes.Name, user.Name!),
                new (ClaimTypes.Email, user.Email!),
                new (ClaimTypes.Role, user.Role!),
            };
            if(!string.IsNullOrEmpty(user.Role) || !Equals("string", user.Role))
            {
                claims.Add(new(ClaimTypes.Role, user.Role!));
            }

            var token = new JwtSecurityToken(
                issuer: config["Authentication:Issuer"],
                audience: config["Authentication:Audience"],
                claims: claims,
                expires: null,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public async Task<Response> Register(AppUserDTO appUserDTO)
        {
            var user = await GetUserByEmail(appUserDTO.Email);
            if (user is not null)
            {
                return new Response(false, "this email already exists");
            }
            var appUser = new AppUser()
            {
                Name = appUserDTO.Name,
                Telephone = appUserDTO.Telephone,
                Address = appUserDTO.Address,
                Email = appUserDTO.Email,
                Role = appUserDTO.Role,
                Password = BCrypt.Net.BCrypt.HashPassword(appUserDTO.Password),
            };

            var result = context!.Users.Add(appUser);
            await context.SaveChangesAsync();
            return result.Entity.ID > 0 ? new Response(true, "user registered successfully") : new Response(false, "invalid data provided");
        }
    }
}
