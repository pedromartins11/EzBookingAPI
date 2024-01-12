using EzBooking.Data;
using EzBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace EzBooking.Repository
{
    public class PaymentRepo
    {
        private readonly DataContext _context;
        public PaymentRepo(DataContext context)
        {
            _context = context;

        }
        public ICollection<Payment> GetPayments()
        {
            return _context.Payments.OrderBy(h => h.id_payment)
            .Include(h => h.state)
            .ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool CreatePayment(Payment payment)
        {
            _context.Add(payment);
            return Save();
        }

        public Payment GetPayment(int paymentId)
        {
            return _context.Payments.Where(p => p.id_payment == paymentId)
            .Include(h => h.state)
            .FirstOrDefault(); ;
        }

        public bool UpdatePayment(Payment payment)
        {
            _context.Update(payment);
            return Save();
        }

        public bool DeletePayment(Payment payment)
        {
            _context.Remove(payment);
            return Save();
        }


    }
}
