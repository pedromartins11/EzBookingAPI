using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EzBooking.Models
{
    public class House
    {
        [Key]
        public int id_house { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string name { get; set; }

        [Required(ErrorMessage = "O campo Número da Porta é obrigatório.")]
        public int doorNumber { get; set; }

        [Required(ErrorMessage = "O campo Número do Andar é obrigatório.")]
        public int floorNumber { get; set; }

        [Range(0, 100000, ErrorMessage = "O preço deve ser um número não negativo.")]
        public double? price { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O preço anual deve ser um número não negativo.")]
        public double? priceyear { get; set; }

        [Required(ErrorMessage = "O campo Número de Hóspedes é obrigatório.")]
        [Range(1, 30, ErrorMessage = "O número de hóspedes deve ser pelo menos 1.")]
        public int guestsNumber { get; set; }

        public int? rooms {  get; set; }
        [Required(ErrorMessage = "O campo Rua é obrigatório.")]
        public string road { get; set; }

        [Required(ErrorMessage = "O campo Avaliação do Imóvel é obrigatório.")]
        public string propertyAssessment { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "O código da porta deve ser pelo menos 1.")]
        public int? codDoor { get; set; }

        [Required(ErrorMessage = "O campo Sala Compartilhada é obrigatório.")]
        public bool sharedRoom { get; set; }

        [Required(ErrorMessage = "O campo Código Postal é obrigatório.")]
        public PostalCode PostalCode { get; set; }

        public StatusHouse? StatusHouse { get; set; }
        public ICollection<Images>? Images { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
        public User? User {  get; set; }

    }
}
