using EzBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace EzBooking.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
            
        }

        public DbSet<House> Houses{ get; set; }
        public DbSet<PostalCode> PostalCodes { get; set; }
        public DbSet<StatusHouse> StatusHouses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationStates> ReservationStates { get; set; }
        
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentStates> PaymentStates { get; set; }

        public DbSet<Images> Images { get; set; }
        public DbSet<UserTypes> UserTypes { get; set; }
        public DbSet<RevokedTokens> RevokedTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<PostalCode>()
                        .Property(p => p.postalCode)
                        .ValueGeneratedNever();

        }
    }
}
