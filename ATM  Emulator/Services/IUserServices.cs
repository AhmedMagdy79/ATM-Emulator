using ATM__Emulator.Dtos;
using ATM__Emulator.Models;

namespace ATM__Emulator.Services
{
    public interface IUserServices
    {
        Task<Response<UserResponseDto>> SignupAsync(UserRequestDto userData);

        bool UserExist(string userName);

        Task<Response<LoginResponseDto>> LoginAsync(UserRequestDto userData);

    }
}
