using ATM__Emulator.Dtos;
using ATM__Emulator.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ATM__Emulator.Services
{
    public class UserServices : IUserServices
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;


        public UserServices(UserManager<User> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<Response<UserResponseDto>> SignupAsync(UserRequestDto userData)
        {

            if (await _userManager.FindByNameAsync(userData.UserName) is not null)
                return Response<UserResponseDto>.CreateErrorResponse("Username is already registered!", null, 400);

            var user = new User
            {
                UserName = userData.UserName,
                Balance = 0,
            };

            var userCreated =  await _userManager.CreateAsync(user, userData.Password);

           var addToRoleResult = await _userManager.AddToRoleAsync(user, "User");

            if(!userCreated.Succeeded && !addToRoleResult.Succeeded)
            {
                return Response<UserResponseDto>.CreateErrorResponse("Something went wrong while creating user",null, 500);
            }

            var result = new UserResponseDto {Id= user.Id ,UserName = user.UserName, Balance = user.Balance };

            return Response<UserResponseDto>.CreateSuccessResponse(result, 201);
        }

        public async Task<Response<LoginResponseDto>> LoginAsync(UserRequestDto userData)
        {
            var user = await _userManager.FindByNameAsync(userData.UserName);

            if (user == null || !await _userManager.CheckPasswordAsync(user, userData.Password))
            {
                return Response<LoginResponseDto>.CreateErrorResponse("Wrong username or password", null, 401);
            }

            
            var jwtSecurityToken = await CreateJwtToken(user);
            var result = new LoginResponseDto
                {
                 Id = user.Id,
                 UserName = user.UserName,
                 Balance = user.Balance,
                 AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
        };
            return Response<LoginResponseDto>.CreateSuccessResponse(result, 200);
        }


        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }


    }
}
