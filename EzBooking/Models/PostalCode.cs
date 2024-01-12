using System.ComponentModel.DataAnnotations;

namespace EzBooking.Models
{

    public class PostalCode
    {
        [Key]
        public int postalCode { get; set; }
        [Required(ErrorMessage = "O campo Concelho é obrigatório.")]
        public string concelho { get; set; }
        [Required(ErrorMessage = "O campo Distrito é obrigatório.")]
        public string district { get; set; }
    }


}