namespace ATM__Emulator.Dtos
{
    public class WithdrawResponseDto
    {
        public int UserId { get; set; }

        public double CurrentBalance { get; set; }

        public double AmountWithdrawed { get; set; }

    }
}
