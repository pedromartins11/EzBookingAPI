using EzBooking.Data;
using EzBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace EzBooking.Repository
{
    public class FeedbackRepo
    {
        private readonly DataContext _context;
        public FeedbackRepo(DataContext context)
        {
            _context = context;

        }
        public ICollection<Feedback> GetFeedbacks()
        {
            return _context.Feedbacks.OrderBy(h => h.id_feedback)
            .Include(h => h.Reservation)
            .ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool CreateFeedback(Feedback feedback)
        {
            _context.Add(feedback);
            return Save();
        }

        public Feedback GetFeedback(int feedbackId)
        {
            return _context.Feedbacks.Where(p => p.id_feedback == feedbackId)
            .Include(h => h.Reservation)
            .FirstOrDefault(); ;
        }

        public bool UpdateFeedback(Feedback feedback)
        {
            _context.Update(feedback);
            return Save();
        }

        public bool DeleteFeedback(Feedback feedback)
        {
            _context.Remove(feedback);
            return Save();
        }


    }
}
