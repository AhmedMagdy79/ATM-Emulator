using ATM__Emulator.Dtos;
using ATM__Emulator.Helper;
using ATM__Emulator.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 

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

        [Authorize]
        [HttpPatch("deposite")]
        public async Task<IActionResult> Deposite(DepositeRequestDto dto)
        {

                var response = await _transactionServices.DepositeAsync(dto);

                if (response.StatusCode == 404) { return NotFound(response); }

                return Ok(response);
        }

        [Authorize]
        [HttpPatch("withdraw")]
        public async Task<IActionResult> Withdraw( WithdrawRequestDto dto)
        {
                var response = await _transactionServices.WithdrawAsync(dto);

                if (response.StatusCode == 404) { return NotFound(response); }

                if (response.StatusCode == 403) { return BadRequest(response); }

                return Ok(response);
        }
    }
}
