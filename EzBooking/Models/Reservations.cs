using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EzBooking.Models
{
    public class Reservation
    {
        [Key]
        public int id_reservation { get; set; }

        [Required(ErrorMessage = "O campo Data Inicial é obrigatório.")]
        public DateTime init_date { get; set; }

        [Required(ErrorMessage = "O campo Data Final é obrigatório.")]
        public DateTime end_date { get; set; }

        [Required(ErrorMessage = "O campo Número de Hóspedes é obrigatório.")]
        [Range(1, 30, ErrorMessage = "O número de hóspedes deve ser pelo menos 1.")]
        public int guestsNumber { get; set; }


        public User? User { get; set; }
        
        public House? House { get; set; }
        
        public ReservationStates? ReservationStates { get; set; }

    }
}