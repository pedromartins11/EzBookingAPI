using EzBooking.Data;
using EzBooking.Models;
using System.Data;

namespace EzBooking.Repository
{
    public class StatusHouseRepo
    {
        private readonly DataContext _context;

        public StatusHouseRepo(DataContext context)
        {
            _context = context;
        }
        public ICollection<StatusHouse> GetStatusHouses()
        {
            return _context.StatusHouses
                .OrderBy(sh => sh.id)
                .ToList();
        }

        public StatusHouse GetStatusHouseById(int id)
        {
            return _context.StatusHouses
                .FirstOrDefault(sh => sh.id == id);
        }

        public bool CreateStatusHouse(StatusHouse statusHouse)
        {
            _context.Add(statusHouse);
            return Save();
        }

        public bool StatusHouseExists(string statusHouse)
        {
            return _context.StatusHouses.Any(sh => sh.name == statusHouse);
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
