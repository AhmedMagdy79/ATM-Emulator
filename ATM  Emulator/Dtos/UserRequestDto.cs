using System.ComponentModel.DataAnnotations;

namespace ATM__Emulator.Dtos
{
    public class UserRequestDto
    {
        [Required(AllowEmptyStrings =false,ErrorMessage ="UserName is Required")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is Required")]
        public string Password { get; set; }

    }
}
