using EzBooking.Data;
using EzBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace EzBooking.Repository
{
    public class PostalCodeRepo
    {
        private readonly DataContext _context;

        public PostalCodeRepo(DataContext context)
        {
            _context = context;
        }

        public ICollection<PostalCode> GetPostalCodes()
        {
            return _context.PostalCodes
                .OrderBy(pc => pc.postalCode)
                .ToList();
        }

        public PostalCode GetPostalCodeById(int id)
        {
            return _context.PostalCodes
                .FirstOrDefault(p => p.postalCode == id);
        }

        public bool PostalCodeExists(int postalcode)
        {
            return _context.PostalCodes.Any(h => h.postalCode == postalcode);
        }

        public bool CreatePostalCode(PostalCode postalcode)
        {
            _context.Add(postalcode);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
