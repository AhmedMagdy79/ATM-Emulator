using ATM__Emulator.Dtos;
using ATM__Emulator.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ATM__Emulator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly IConfiguration _config;

        public UserController(IUserServices userServices, IConfiguration config)
        {
            _userServices = userServices;
            _config = config;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(UserRequestDto dto)
        {
            bool isExist = _userServices.UserExist(dto.UserName);

            if (isExist)
            {
                return BadRequest("Username already exists");
            }

            var user = await _userServices.Signup(dto);

            var response = new UserResponseDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Balance = user.Balance,
            };

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserRequestDto dto)
        {
            try
            {

                var user = await _userServices.Login(dto);

                return Ok(user);
            }catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        /*
        private bool ValidateAccessToken (string accessToken)
        {
            try
            {
                // Validate the token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]); 
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _config["Jwt:Issuer"], 
                    ValidateAudience = true,
                    ValidAudience = _config["Jwt:Issuer"],
                    ValidateLifetime = true
                };

                ClaimsPrincipal principal = tokenHandler.ValidateToken(accessToken, validationParameters, out SecurityToken validatedToken);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return false;
            }
            
        }*/
    }
}
