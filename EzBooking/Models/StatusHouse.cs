using System.ComponentModel.DataAnnotations;

namespace EzBooking.Models
{
    public class StatusHouse
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
    }
}