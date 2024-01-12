using System.ComponentModel.DataAnnotations;

namespace EzBooking.Models
{
    public class RevokedTokens
    {
        [Key]
        public int id_revokedToken { get; set; }
        public string token {  get; set; }
        public DateTime revocationDate {  get; set; }
    }
}