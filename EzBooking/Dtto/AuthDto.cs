using System.ComponentModel.DataAnnotations;

namespace EzBooking.Dtto
{
    public class LoginDto
    {
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        public string email { get; set; }

        [Required(ErrorMessage = "O campo Password é obrigatório.")]
        public string password { get; set; }
    }
}
