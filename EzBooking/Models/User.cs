using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EzBooking.Models
{
    public class User
    {
        [Key]
        public int id_user { get; set; }
        [Required(ErrorMessage = "O campo Nome � obrigat�rio.")]
        public string name { get; set; }
        [Required(ErrorMessage = "O campo Email � obrigat�rio.")]
        public string email { get; set; }
        [Required(ErrorMessage = "O campo Password � obrigat�rio.")]
        public string password { get; set; }
        [Required(ErrorMessage = "O campo Phone � obrigat�rio.")]
        public int phone { get; set; }
        public string? token { get; set; }
        public bool status { get; set; }
        public string? image { get; set; }
        public UserTypes? userType { get; set; }
        public ICollection<House>? Houses { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }

    }
}