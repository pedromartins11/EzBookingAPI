using System.ComponentModel.DataAnnotations;

namespace EzBooking.Models
{
    public class PaymentStates
    {
        [Key]
        public int id_paymentStates { get; set; }
        public string state { get; set; }
    }
}
