using System.ComponentModel.DataAnnotations;

namespace EzBooking.Models
{
    public class ReservationStates
    {
        [Key]
        public int id { get; set; }
        public string state { get; set; }
    }
}