namespace ATM__Emulator.Dtos
{
    public class WithdrawResponseDto
    {
        public string UserId { get; set; }

        public decimal CurrentBalance { get; set; }

        public decimal AmountWithdrawed { get; set; }

    }
}
