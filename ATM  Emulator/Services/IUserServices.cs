using ATM__Emulator.Dtos;
using ATM__Emulator.Models;

namespace ATM__Emulator.Services
{
    public interface IUserServices
    {
        Task<User> Signup(UserRequestDto userData);

        bool UserExist(string userName);

        Task<LoginResponseDto> Login(UserRequestDto userData);

    }
}
