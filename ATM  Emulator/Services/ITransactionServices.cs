using ATM__Emulator.Dtos;

namespace ATM__Emulator.Services
{
    public interface ITransactionServices
    {
        Task<WithdrawResponseDto> Withdraw(WithdrawRequestDto withdrawData);

        Task<DepositeResponseDto> Deposite(DepositeRequestDto depositeData);
    }
}
