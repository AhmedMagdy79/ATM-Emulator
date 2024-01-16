using ATM__Emulator.Dtos;

namespace ATM__Emulator.Services
{
    public interface ITransactionServices
    {
        Task<Response<WithdrawResponseDto>> WithdrawAsync(WithdrawRequestDto withdrawData);

        Task<Response<DepositeResponseDto>> DepositeAsync(DepositeRequestDto depositeData);
    }
}
