using System.ComponentModel.DataAnnotations;

namespace EzBooking.Models
{
    public class Feedback
    {
        [Key]
        public int id_feedback { get; set; }
        [Range(1, 5, ErrorMessage = "A classificação deverá ser entre 1 e 5.")]
        public int classification { get; set; }
        public string? comment { get; set; }
        public Reservation? Reservation { get; set; }
    }
}