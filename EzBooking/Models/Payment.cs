using System.ComponentModel.DataAnnotations;

namespace EzBooking.Models
{
    public class Payment
    {
        [Key]
        public int id_payment { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime? paymentDate { get; set; } 
        public string? paymentMethod { get; set; } 
        public float paymentValue { get; set; }

        public PaymentStates state { get; set; }
        public Reservation Reservation { get; set; }

    }
}
