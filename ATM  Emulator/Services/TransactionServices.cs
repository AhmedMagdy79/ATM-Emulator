using ATM__Emulator.Dtos;
using ATM__Emulator.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ATM__Emulator.Services
{
    public class TransactionServices : ITransactionServices
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;


        public TransactionServices(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Response<DepositeResponseDto>> DepositeAsync(string userId, decimal amount)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) { return Response<DepositeResponseDto>.CreateErrorResponse("user not found", null, 404); }

            user.Balance += amount;

            var updatedUser = await _userManager.UpdateAsync(user);

            if (!updatedUser.Succeeded) 
            {
                return Response<DepositeResponseDto>.CreateErrorResponse("Something went wrong", null, 500);
            }

            var transaction = new Transaction
            {
                UserId = user.Id,
                TransactionType = "Deposite",
                TransactionAmount = amount,
                TransactionTime = DateTime.Now
            };

            await _context.AddAsync(transaction);

            await _context.SaveChangesAsync();

            var result= new DepositeResponseDto { UserId = user.Id, CurrentBalance = user.Balance };

            return Response<DepositeResponseDto>.CreateSuccessResponse(result, 200);

        }

        public async Task<Response<WithdrawResponseDto>> WithdrawAsync(string userId, decimal amount)
        {

            var user = await _userManager.FindByIdAsync(userId);


            if (user == null) { return Response<WithdrawResponseDto>.CreateErrorResponse("user not found", null, 404); }

            if (user.Balance < amount)
            {
                return Response<WithdrawResponseDto>.CreateErrorResponse("Your balance isn't enough", null, 403);
            }

            user.Balance -= amount;

            var updatedUser = await _userManager.UpdateAsync(user);

            if (!updatedUser.Succeeded)
            {
                return Response<WithdrawResponseDto>.CreateErrorResponse("Something went wrong", null, 500);
            }

            var transaction = new Transaction
            {
                UserId = user.Id,
                TransactionType = "Withdraw",
                TransactionAmount = amount,
                TransactionTime = DateTime.Now,
            };

            await _context.AddAsync(transaction);
            await _context.SaveChangesAsync();

            var result = new WithdrawResponseDto
            {
                UserId = user.Id,
                AmountWithdrawed = amount,
                CurrentBalance = user.Balance
            };

            return Response<WithdrawResponseDto>.CreateSuccessResponse(result, 200);
        }
    }
}
