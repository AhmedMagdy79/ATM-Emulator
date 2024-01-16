using ATM__Emulator.Dtos;
using ATM__Emulator.Models;
using Microsoft.EntityFrameworkCore;

namespace ATM__Emulator.Services
{
    public class TransactionServices : ITransactionServices
    {

        private readonly ApplicationDbContext _context;


        public TransactionServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<DepositeResponseDto>> DepositeAsync(DepositeRequestDto depositeData)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == depositeData.UserId);

            if (user == null) { return Response<DepositeResponseDto>.CreateErrorResponse("user not found", null, 404); }

            user.Balance += depositeData.Amount;

            var transaction = new Transaction
            {
                UserId = user.Id,
                TransactionType = "Deposite",
                TransactionAmount = depositeData.Amount,
                TransactionTime = DateTime.Now
            };

            await _context.AddAsync(transaction);

            await _context.SaveChangesAsync();

            var result= new DepositeResponseDto { UserId = user.Id, CurrentBalance = user.Balance };

            return Response<DepositeResponseDto>.CreateSuccessResponse(result, 200);

        }

        public async Task<Response<WithdrawResponseDto>> WithdrawAsync(WithdrawRequestDto withdrawData)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == withdrawData.UserId);

            if (user == null) { return Response<WithdrawResponseDto>.CreateErrorResponse("user not found", null, 404); }

            if(user.Balance < withdrawData.Amount)
            {
                return Response<WithdrawResponseDto>.CreateErrorResponse("Your balance isn't enough", null, 403);
            }

            var oldBalance = user.Balance;
            user.Balance -= withdrawData.Amount;

            DateTime dateTime = DateTime.Now;

            var transaction = new Transaction
            {
                UserId = user.Id,
                TransactionType = "Withdraw",
                TransactionAmount = withdrawData.Amount,
                TransactionTime = dateTime,
            };

            await _context.AddAsync(transaction);
             await _context.SaveChangesAsync();

            var amountWithdrawed = oldBalance - user.Balance;
            var result = new WithdrawResponseDto
            {
                UserId = user.Id,
                AmountWithdrawed = amountWithdrawed,
                CurrentBalance = user.Balance
            };

            return Response<WithdrawResponseDto>.CreateSuccessResponse(result, 200);
        }
    }
}
