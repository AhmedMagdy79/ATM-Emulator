using ATM__Emulator.Dtos;
using ATM__Emulator.Models;
using Microsoft.EntityFrameworkCore;

namespace ATM__Emulator.Services
{
    public class TransactionServices : ITransactionServices
    {

        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;


        public TransactionServices(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<DepositeResponseDto> Deposite(DepositeRequestDto depositeData)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == depositeData.UserId);

            if (user == null) { throw new Exception("User not found"); }

            user.Balance += depositeData.Amount;
            _context.SaveChanges();

            DateTime dateTime = DateTime.Now;

            var transaction = new Transaction
            {
                UserId = user.Id,
                TransactionType = "Deposite",
                TransactionAmount = depositeData.Amount,
                TransactionTime = dateTime
            };

            await _context.AddAsync(transaction);

            _context.SaveChanges();

            return new DepositeResponseDto { UserId = user.Id, CurrentBalance = user.Balance };

        }

        public async Task<WithdrawResponseDto> Withdraw(WithdrawRequestDto withdrawData)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == withdrawData.UserId);

            if (user == null) { throw new Exception("User not found"); }

            var oldBalance = user.Balance;
            user.Balance -= withdrawData.Amount;
            _context.SaveChanges();


            DateTime dateTime = DateTime.Now;

            var transaction = new Transaction
            {
                UserId = user.Id,
                TransactionType = "Withdraw",
                TransactionAmount = withdrawData.Amount,
                TransactionTime = dateTime,
            };

            await _context.AddAsync(transaction);

            _context.SaveChanges();

            var amountWithdrawed = oldBalance - user.Balance;
            return new WithdrawResponseDto
            {
                UserId = user.Id,
                AmountWithdrawed = amountWithdrawed,
                CurrentBalance = user.Balance
            };
        }
    }
}
