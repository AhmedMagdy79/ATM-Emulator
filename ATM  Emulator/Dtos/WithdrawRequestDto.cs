using System.ComponentModel.DataAnnotations;

namespace ATM__Emulator.Dtos
{
    public class WithdrawRequestDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "User Id Must be Positive")]
        public int UserId { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Amount Must be Positive")]
        public decimal Amount { get; set; }
    }
}
