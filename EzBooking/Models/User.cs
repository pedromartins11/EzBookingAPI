using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EzBooking.Models
{
    public class User
    {
        [Key]
        public int id_user { get; set; }
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string name { get; set; }
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        public string email { get; set; }
        [Required(ErrorMessage = "O campo Password é obrigatório.")]
        public string password { get; set; }
        [Required(ErrorMessage = "O campo Phone é obrigatório.")]
        public int phone { get; set; }
        public string? token { get; set; }
        public bool status { get; set; }
        public string? image { get; set; }
        public UserTypes? userType { get; set; }
        public ICollection<House>? Houses { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }

    }
}