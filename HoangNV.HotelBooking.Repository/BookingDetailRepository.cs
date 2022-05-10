using HoangNV.HotelBooking.Core.Repository;
using HoangNV.HotelBooking.Entities.Entities;
using HoangNV.HotelBooking.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Repository
{
    public class BookingDetailRepository : Repository<BookingDetails>, IBookingDetailRepository
    {
        private readonly BookingContext _dbContext;
        public BookingDetailRepository(BookingContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Insert(BookingDetails bookingDetails)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await _dbContext.BookingDetails.AddAsync(bookingDetails);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
            return true;
        }

        public async Task<IEnumerable<BookingDetails>> GetBookingDetailsByRoomId(int id)
        {
            return await _dbContext.BookingDetails.Include(x => x.Booking).Where(x => x.RoomId == id).ToListAsync();
        }
    }
}
