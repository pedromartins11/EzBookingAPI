using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EzBooking.Models
{
    public class House
    {
        [Key]
        public int id_house { get; set; }

        [Required(ErrorMessage = "O campo Nome � obrigat�rio.")]
        public string name { get; set; }

        [Required(ErrorMessage = "O campo N�mero da Porta � obrigat�rio.")]
        public int doorNumber { get; set; }

        [Required(ErrorMessage = "O campo N�mero do Andar � obrigat�rio.")]
        public int floorNumber { get; set; }

        [Range(0, 100000, ErrorMessage = "O pre�o deve ser um n�mero n�o negativo.")]
        public double? price { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O pre�o anual deve ser um n�mero n�o negativo.")]
        public double? priceyear { get; set; }

        [Required(ErrorMessage = "O campo N�mero de H�spedes � obrigat�rio.")]
        [Range(1, 30, ErrorMessage = "O n�mero de h�spedes deve ser pelo menos 1.")]
        public int guestsNumber { get; set; }

        public int? rooms {  get; set; }
        [Required(ErrorMessage = "O campo Rua � obrigat�rio.")]
        public string road { get; set; }

        [Required(ErrorMessage = "O campo Avalia��o do Im�vel � obrigat�rio.")]
        public string propertyAssessment { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "O c�digo da porta deve ser pelo menos 1.")]
        public int? codDoor { get; set; }

        [Required(ErrorMessage = "O campo Sala Compartilhada � obrigat�rio.")]
        public bool sharedRoom { get; set; }

        [Required(ErrorMessage = "O campo C�digo Postal � obrigat�rio.")]
        public PostalCode PostalCode { get; set; }

        public StatusHouse? StatusHouse { get; set; }
        public ICollection<Images>? Images { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
        public User? User {  get; set; }

    }
}
