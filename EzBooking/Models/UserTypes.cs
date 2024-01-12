using System.ComponentModel.DataAnnotations;

namespace EzBooking.Models
{
    public class UserTypes
    {
        [Key]
        public int id_userType { get; set; }
        [Required(ErrorMessage = "O campo Type é obrigatório.")]
        public string type { get; set; }

    }
}
