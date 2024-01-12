using System.ComponentModel.DataAnnotations;

namespace EzBooking.Models
{

    public class PostalCode
    {
        [Key]
        public int postalCode { get; set; }
        [Required(ErrorMessage = "O campo Concelho � obrigat�rio.")]
        public string concelho { get; set; }
        [Required(ErrorMessage = "O campo Distrito � obrigat�rio.")]
        public string district { get; set; }
    }


}