using ATM__Emulator.Dtos;
using ATM__Emulator.Helper;
using ATM__Emulator.Services;
using Microsoft.AspNetCore.Mvc;


namespace ATM__Emulator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExceptionFilter]
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
                var result = Response<UserResponseDto>.CreateErrorResponse("Username already exists", null, 403);
                return BadRequest(result);
            }

            var response = await _userServices.SignupAsync(dto);

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserRequestDto dto)
        {

                var response = await _userServices.LoginAsync(dto);

                if (response.StatusCode == 403) { return BadRequest(response); }

                return Ok(response);
        }

    }
}
