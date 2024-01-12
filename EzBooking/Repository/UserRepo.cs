using EzBooking.Data;
using EzBooking.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace EzBooking.Repository
{
    public class UserRepo
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public UserRepo(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }
        public async Task<ICollection<User>> GetUsers()
        {
            return await _context.Users.OrderBy(h => h.id_user)
                .Include(h => h.userType)
                .ToListAsync();
        }

        public async Task<User> GetHousesFromUser(int id)
        {
            return await _context.Users
                .Include(h => h.userType)
                .Include(h => h.Houses)
                    .ThenInclude(h => h.PostalCode)
                .Include(h => h.Houses)
                    .ThenInclude(h => h.StatusHouse)
                .Include(h => h.Houses)
                    .ThenInclude(h => h.Images)
                .FirstOrDefaultAsync(h => h.id_user == id);
        }
        public async Task<ICollection<User>> GetUsers()
        {
            return await _context.Users.OrderBy(h => h.id_user)
                .Include(h => h.userType)
                .ToListAsync();
        }

        public async Task<User> GetHousesFromUser(int id)
        {
            return await _context.Users
                .Include(h => h.userType)
                .Include(h => h.Houses)
                    .ThenInclude(h => h.PostalCode)
                .Include(h => h.Houses)
                    .ThenInclude(h => h.StatusHouse)
                .Include(h => h.Houses)
                    .ThenInclude(h => h.Images)
                .FirstOrDefaultAsync(h => h.id_user == id);
        }

        public async Task<Reservation> GetReservationsFromUser(int id)
        {
            return await _context.Reservations
                .Include(u => u.ReservationStates)
                .Include(h => h.House)
                    .ThenInclude(h=>h.StatusHouse)
                .Include(h => h.House)
                    .ThenInclude(h => h.PostalCode)
                .Include(h => h.House)
                    .ThenInclude(h => h.Images)
                .FirstOrDefaultAsync(u => u.User.id_user == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool CreateUser(User user)
        {
            _context.Add(user);
            return Save();
        }

        public bool CheckEmail(string email)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.email == email);
            return existingUser != null;
        }

        public bool UserExists(int userId)
        {
            return _context.Users.Any(p => p.id_user == userId);
        }

        public bool DeleteUser(User user)
        {
            _context.Remove(user);
            return Save();
        }

        public User GetUser(int userId)
        {
            return _context.Users.Where(p => p.id_user == userId).FirstOrDefault();
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.Where(p => p.email == email).FirstOrDefault();
        }

        public bool UpdateUser(User user)
        {
                _context.Update(user);
                return Save();
        }

        public string CreateToken(User user)
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.email)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public bool TokenIsRevoked(string token)
        {
            
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();

                string query = "SELECT COUNT(*) FROM RevokedTokens WHERE Token = @token";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@token", token);

                    int count = (int)cmd.ExecuteScalar();

                    return count > 0;
                }
            }
        }

        public bool IsTokenValid(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                
                if (jsonToken?.ValidTo != null && jsonToken.ValidTo < DateTime.UtcNow)
                {
                    return false;
                }


                return true;
            }
            catch (Exception)
            {
                return false; // Token inválido
            }
        }

    }
}
