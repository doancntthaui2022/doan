using HoangNV.HotelBooking.Entities.Entities;
using HoangNV.HotelBooking.Repository.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Repository
{
    public class BookingContext : DbContext
    {
        public BookingContext(DbContextOptions<BookingContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<HotelBranchs>().HasData(new Entities.Entities.HotelBranchs
            {
                HotelBranchId=1,
                HotelBranchCode="HNVHotel",
                HotelBranchName="Hotel",
                Status= (int)HotelEnum.Active,
                Address="Hà Nội",
                Description="None",
              
            });
            modelBuilder.Entity<Roles>().HasData(new Roles
            {
                RoleId = "6ea013dd-40ea-4a0e-9d0e-a675c0e9498a",
                RoleName = "Admin",
            });
            modelBuilder.Entity<Roles>().HasData(new Roles
            {
                RoleId = "6ea013dd-40ea-4a0e-9d0e-a675c0e9498e",
                RoleName = "Employee",
            });
            modelBuilder.Entity<Roles>().HasData(new Roles
            {
                RoleId = "6ea013dd-40ea-4a0e-9d0e-a675c0e9498f",
                RoleName = "Client",
            });
            var user = new Users
            {
                Id = "6ea013dd-40ea-4a0e-9d0e-a675c0e9498b",
                FullName = "Admin",
                UserName = "Admin",
                Active=1,
                RoleId = "6ea013dd-40ea-4a0e-9d0e-a675c0e9498a",
            };
            PasswordHasher<Users> passwordHasher = new PasswordHasher<Users>();
            user.PassWord= passwordHasher.HashPassword(user, "Admin@123");
            modelBuilder.Entity<Users>().HasData(user);
            
        }

        public DbSet<Beds> Beds { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingDetails> BookingDetails { get; set; }
        public DbSet<Convenients> Convenients { get; set; }
        public DbSet<ConvenientTypes> ConvenientTypes { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<HotelBranchs> HotelBranchs { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<RoomBeds> RoomBeds { get; set; }
        public DbSet<RoomConvenients> RoomConvenients { get; set; }
        public DbSet<Rooms> Rooms { get; set; }
        public DbSet<RoomTypes> RoomTypes { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
    }
}
