using ATM__Emulator.Helper;
using ATM__Emulator.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ATM__Emulator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExceptionFilter]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionServices _transactionServices;

        public TransactionController(ITransactionServices transactionServices)
        {
            _transactionServices = transactionServices;
        }

        [Authorize(Roles = "User")]
        [HttpPatch("deposite")]
        public async Task<IActionResult> Deposite( decimal amount)
        {
            if (!isAmountInRange(amount))
                return BadRequest("Amount Value Must be Positive");

            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var userId = identity.FindFirst("uid");

            if (userId == null) { return Unauthorized(); }

            var response = await _transactionServices.DepositeAsync(userId.Value, amount);

             if (response.StatusCode == 404) { return NotFound(response); }

             if (response.StatusCode == 500) { return BadRequest(response); }

             return Ok(response);
        }

        [HttpPatch("withdraw")]
        public async Task<IActionResult> Withdraw(decimal amount)
        {
            if (!isAmountInRange(amount))
                return BadRequest("Amount Value Must be Positive");

            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var userId = identity.FindFirst("uid");

            if (userId == null) { return Unauthorized(); }

            var response = await _transactionServices.WithdrawAsync(userId.Value,amount);

            if (response.StatusCode == 404) { return NotFound(response); }

            if (response.StatusCode == 403) { return BadRequest(response); }

            if (response.StatusCode == 500) { return BadRequest(response); }

            return Ok(response);

        }

        private bool isAmountInRange(decimal amount)
        {
            return decimal.MaxValue >= amount && amount >= 1;
        }
    }
}
