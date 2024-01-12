using EzBooking.Data;
using EzBooking.Dtto;
using EzBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace EzBooking.Repository
{
    public class HouseRepo
    {
        private readonly DataContext _context;
        public HouseRepo(DataContext context)
        {
            _context = context;

        }

        public async Task<ICollection<object>> AvailableHouses(string location, int guestsNumber, DateTime startDate, DateTime endDate)
        {
            var houses = await _context.Houses
                .Where(h => h.PostalCode.district == location &&
                       h.guestsNumber >= guestsNumber &&
                       (!h.Reservations.Any() ||
                       h.Reservations.All(reservation =>
                            reservation.ReservationStates.id == 1 || reservation.ReservationStates.id == 3 || reservation.ReservationStates.id == 4 || reservation.ReservationStates.id == 5 ||
                            reservation.end_date <= startDate || reservation.init_date >= endDate))
                       )
                .Select(h => new
                {
                    h.id_house,
                    h.name,
                    h.price,
                    h.priceyear,
                    h.rooms,
                    h.guestsNumber,
                    h.road,
                    h.sharedRoom,
                    PostalCode = new
                    {
                        h.PostalCode.postalCode,
                        h.PostalCode.concelho,
                        h.PostalCode.district
                    },
                    Images = h.Images.Select(img => new
                    {
                        img.image
                    }).ToList()
                })
                .OrderBy(h => h.id_house)
                .ToListAsync();

            return houses.ToList<object>();
        }

        //Mostra casas que estão disponiveis
        public async Task<ICollection<House>> GetHouses()
        {
            return await _context.Houses.Where(h => h.StatusHouse.id == 2)
                .Include(h => h.PostalCode)
                .Include(h => h.StatusHouse)
                .Include(h => h.Images)
                .OrderBy(h => h.id_house)
                .ToListAsync();
        }

        public async Task<House> GetHouseById(int id)
        {
            return await _context.Houses
                .Include(h => h.PostalCode)
                .Include(h => h.StatusHouse)
                .Include(h => h.Images)
                .FirstOrDefaultAsync(h => h.id_house == id);
        }

        public async Task<object> GetHouseByIdDetail(int id)
        {
            return await _context.Houses
                .Where(h => h.id_house == id)
                .Select(h => new
                {
                    h.id_house,
                    h.name,
                    h.doorNumber,
                    h.floorNumber,
                    h.price,
                    h.priceyear,
                    h.rooms,
                    h.guestsNumber,
                    h.road,
                    h.sharedRoom,
                    PostalCode = new
                    {
                        h.PostalCode.postalCode,
                        h.PostalCode.concelho,
                        h.PostalCode.district
                    },
                    Images = h.Images.Select(img => new
                    {
                        img.image
                    }).ToList()
                })
                .SingleOrDefaultAsync();
        }


        public ICollection<House> GetHousesSusp()
        {
            return _context.Houses.Where(h => h.StatusHouse.id == 1)
                .Include(h => h.PostalCode)
                .Include(h => h.Images)
                .ToList();
        }

        public async Task<House> GetReservationFromHouseById(int id)
        {
            return await _context.Houses
                .Include(h => h.PostalCode)
                .Include(h => h.StatusHouse)
                .Include(h => h.Images)
                .Include(h => h.Reservations)
                .FirstOrDefaultAsync(h => h.id_house == id);
        }

        //public decimal GetHouseRating(int id) 
        //{ 
        //    var review = _context
        //}
        //var postalcode = _context.PostalCodes.Where(pc => pc.postalCode == house.PostalCode.postalCode).FirstOrDefault();

        public async Task<bool> CreateHouse(House house)
        { 
            await _context.AddAsync(house);
            return Save();
        }


        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool PostalCodePropertyExists(int postalCode, string propertyAssessment)
        {
            return _context.Houses.Any(h => h.PostalCode.postalCode == postalCode && h.propertyAssessment == propertyAssessment);
        }

        public bool HouseExists(int houseid)
        {
            return _context.Houses.Any(h => h.id_house == houseid);
        }

        public bool UpdateHouse(House house)
        {

            _context.Update(house);
            return Save();
        }

        public bool DeleteHouse(House house)
        {
            _context.Remove(house);
            return Save();
        }
    }
}
