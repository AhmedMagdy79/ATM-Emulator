using ATM__Emulator.Dtos;

namespace ATM__Emulator.Services
{
    public interface ITransactionServices
    {
        Task<Response<WithdrawResponseDto>> WithdrawAsync(string userId, decimal amount);

        Task<Response<DepositeResponseDto>> DepositeAsync(string userId, decimal amount);
    }
}
