using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EzBooking.Models
{
    public class Reservation
    {
        [Key]
        public int id_reservation { get; set; }

        [Required(ErrorMessage = "O campo Data Inicial � obrigat�rio.")]
        public DateTime init_date { get; set; }

        [Required(ErrorMessage = "O campo Data Final � obrigat�rio.")]
        public DateTime end_date { get; set; }

        [Required(ErrorMessage = "O campo N�mero de H�spedes � obrigat�rio.")]
        [Range(1, 30, ErrorMessage = "O n�mero de h�spedes deve ser pelo menos 1.")]
        public int guestsNumber { get; set; }


        public User? User { get; set; }
        
        public House? House { get; set; }
        
        public ReservationStates? ReservationStates { get; set; }

    }
}