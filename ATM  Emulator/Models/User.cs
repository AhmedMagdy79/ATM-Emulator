using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATM__Emulator.Models
{
    public class User : IdentityUser
    {

        public decimal Balance { get; set; }

    }
}
