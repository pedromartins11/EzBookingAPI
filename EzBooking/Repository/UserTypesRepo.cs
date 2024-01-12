using EzBooking.Data;
using EzBooking.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace EzBooking.Repository
{
    public class UserTypesRepo
    {
        private readonly DataContext _context;

        public UserTypesRepo(DataContext context)
        {
            _context = context;

        }
        public ICollection<UserTypes> GetUserTypes()
        {
            return _context.UserTypes.OrderBy(h => h.id_userType).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool CreateUserTypes(UserTypes userType)
        {
            _context.Add(userType);
            return Save();
        }

        public bool DeleteUserType(UserTypes userType)
        {
            _context.Remove(userType);
            return Save();
        }

        public UserTypes GetUserType(int userTypeId)
        {
            return _context.UserTypes.Where(p => p.id_userType == userTypeId).FirstOrDefault();
        }

        public bool UserTypeExists(int userTypeId)
        {
            return _context.UserTypes.Any(p => p.id_userType == userTypeId);
        }
        public bool UpdateUserType(UserTypes userType)
        {
            _context.Update(userType);
            return Save();
        }

        public bool CheckType(string type)
        {
            var existingType = _context.UserTypes.FirstOrDefault(u => u.type == type);
            return existingType != null;
        }


    }
}
