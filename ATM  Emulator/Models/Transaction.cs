using System.ComponentModel.DataAnnotations.Schema;

namespace ATM__Emulator.Models
{
    public class Transaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime TransactionTime { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public string TransactionType { get; set; }

        public double TransactionAmount { get; set; }
    }
}
