namespace ATM__Emulator.Dtos
{
    public class WithdrawResponseDto
    {
        public int UserId { get; set; }

        public decimal CurrentBalance { get; set; }

        public decimal AmountWithdrawed { get; set; }

    }
}
