using EzBooking.Models;

namespace EzBooking.Dtto
{
    public class HouseDetailDto
    {
        public string name { get; set; }
        public int doorNumber { get; set; }
        public int floorNumber { get; set; }
        public decimal price { get; set; }
        public decimal? priceyear { get; set; }
        public int guestsNumber { get; set; }
        public int rooms { get; set; }
        public string road { get; set; }
        public string propertyAssessment { get; set; }
        public string codDoor { get; set; }
        public bool sharedRoom { get; set; }

        public PostalCode PostalCode { get; set; }
        public StatusHouse StatusHouse { get; set; }
        public ICollection<Images>? Images { get; set; }
    }
}
