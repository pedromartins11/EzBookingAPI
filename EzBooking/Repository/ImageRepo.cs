using EzBooking.Data;
using EzBooking.Models;

namespace EzBooking.Repository
{
    public class ImageRepo
    {
        private readonly DataContext _context;
        public ImageRepo(DataContext context)
        {
            _context = context;

        }

        public async Task<bool> CreateImage(Images image)
        {
            await _context.AddAsync(image);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
