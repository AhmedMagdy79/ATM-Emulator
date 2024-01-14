using ATM__Emulator.Dtos;
using ATM__Emulator.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ATM__Emulator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionServices _transactionServices;

        public TransactionController(ITransactionServices transactionServices)
        {
            _transactionServices = transactionServices;
        }

        [Authorize]
        [HttpPatch("deposite")]
        public IActionResult Deposite(DepositeRequestDto dto)
        {
            try
            {
                var response = _transactionServices.Deposite(dto);
                return Ok(response);

            }
            catch (Exception ex) { return NotFound(ex.Message); }
        }

        [Authorize]
        [HttpPatch("withdraw")]
        public IActionResult Withdraw( WithdrawRequestDto dto)
        {
            try
            {
                
                var response = _transactionServices.Withdraw(dto);
                return Ok(response);

            }
            catch (Exception ex) { return NotFound(ex.Message); }
        }
    }
}
