using EzBooking.Data;
using EzBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace EzBooking.Repository
{
    public class ReservationRepo
    {   
        private readonly DataContext _context;
        public ReservationRepo(DataContext context) 
        { 
            _context = context;

        }
        public async Task<ICollection<Reservation>> GetReservations()
        {
                return await _context.Reservations
                .Include(r => r.House)
                .Include(r => r.User)
                .Include(r => r.ReservationStates)
                .OrderBy(r => r.id_reservation)
                .ToListAsync();
        }

        public async Task<Reservation> GetReservationById(int id)
        {
            return await _context.Reservations
                .Include(r => r.House)
                .Include(r => r.User)
                .Include(r => r.ReservationStates)
                .FirstOrDefaultAsync(r => r.id_reservation == id);

        }

        public async Task<bool> CreateReservation(Reservation reservation)
        {
            await _context.AddAsync(reservation);
            return Save();
        }

        //public async Task UpdateStateReservationsByDates()
        //{
        //    var now = DateTime.UtcNow;

        //    var expiredReservations = await _context.Reservations
        //        .Where(r => r.ReservationStates.id == 2 && r.end_date <= now)
        //        .ToListAsync();

        //    foreach (var reservation in expiredReservations)
        //    {
        //        reservation.ReservationStates.id = 3; 
        //        _context.Entry(reservation).State = EntityState.Modified;
        //    }

        //    await _context.SaveChangesAsync();
        //}

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool ReservationExists(int reservationid)
        {
            return _context.Reservations.Any(r => r.id_reservation == reservationid);
        }


        //UPDATE E DELETE 
        public bool UpdateReservation(Reservation reservation)
        {

            _context.Update(reservation);
            return Save();
        }

        public bool DeleteReservation(Reservation reservation)
        {
            _context.Remove(reservation);
            return Save();
        }

        // VALIDAR DATA DE RESERVA
        public async Task<bool> ValidateReservationDate(Reservation reservation)
        {
            var reservations = await _context.Reservations
                //.Where(r => r.House == reservation.House && r.ReservationStates.state != 1)
                .Where(r => r.House == reservation.House)
                .ToListAsync();

            foreach (Reservation r in reservations)
            {
                if (r.init_date < reservation.end_date && r.end_date > reservation.init_date)
                {
                    return false; // As datas sobrepõem, logo não estão disponíveis
                }
            }

            return true; // As datas estão disponíveis
        }



        //CALCULAR PRECO TOTAL
        public async Task<double?> CalculateTotalPrice(Reservation reservation)
        {
            double? priceNight = reservation.House.price;

            DateTime checkInDate = reservation.init_date;
            DateTime checkOutDate = reservation.end_date;
            //int nights = Math.Floor((checkOutDate - checkInDate) / (1000 * 60 * 60 * 24));
            TimeSpan difference = checkOutDate - checkInDate;
            int nights = (int)Math.Ceiling(difference.TotalDays); // Usando Ceiling para arredondar para cima


            double? total = priceNight * nights;

            return total;
        }
    }
}
