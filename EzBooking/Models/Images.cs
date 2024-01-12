using System.ComponentModel.DataAnnotations;

namespace EzBooking.Models
{
    public class Images
    {
        [Key]
        public int id_image { get; set; }

        public string image { get; set; }

        public House House { get; set; }

    }
}
