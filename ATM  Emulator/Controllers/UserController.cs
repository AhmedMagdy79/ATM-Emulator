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

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(UserRequestDto dto)
        {

            var response = await _userServices.SignupAsync(dto);

            if (response.StatusCode >= 400) { BadRequest(response); }

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserRequestDto dto)
        {

                var response = await _userServices.LoginAsync(dto);

                if (response.StatusCode == 401) { return Unauthorized(response); }

                return Ok(response);
        }

    }
}
