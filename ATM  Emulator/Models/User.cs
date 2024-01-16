using System.ComponentModel.DataAnnotations.Schema;

namespace ATM__Emulator.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; } 

        public string Salt { get; set; }

        public decimal Balance { get; set; }
    }
}
