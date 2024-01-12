using EzBooking.Data;
using EzBooking.Models;
using System.Data;

namespace EzBooking.Repository
{
    public class ReservationStatesRepo
    {
        private readonly DataContext _context;

        public ReservationStatesRepo(DataContext context)
        {
            _context = context;
        }
        public ICollection<ReservationStates> GetReservationStates()
        {
            return _context.ReservationStates
                .OrderBy(rs => rs.id)
                .ToList();
        }

        public ReservationStates GetReservationStatesById(int id)
        {
            return _context.ReservationStates
                .FirstOrDefault(rs => rs.id == id);
        }

        public bool CreateReservationStates(ReservationStates reservationStates)
        {
            _context.Add(reservationStates);
            return Save();
        }

        public bool ReservationStatesExists(string reservationStates)
        {
            return _context.ReservationStates.Any(rs => rs.state == reservationStates);
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
