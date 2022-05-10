using HoangNV.HotelBooking.Core.Repository;
using HoangNV.HotelBooking.Core.Repository.Interface;
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
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private readonly BookingContext _dbContext;
        public BookingRepository(BookingContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Booking>> Search()
        {
            return await _dbContext.Bookings.Include(x=>x.BookingDetails).ToListAsync();
        }

        public async Task<bool> Insert(Booking booking)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await _dbContext.Bookings.AddAsync(booking);
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

        public async Task<IEnumerable<Booking>> SearchConfirm()
        {
            return await _dbContext.Bookings.Include(x => x.BookingDetails)
                .Include(x=>x.Customer).Where(x=>x.BookingStatus==1).ToListAsync();
        }

        public async Task<Booking> GetByIdInclude(string id)
        {
            return await _dbContext.Bookings.Include(x => x.Customer).Include(x => x.BookingDetails).FirstOrDefaultAsync(x => x.BookingId == id);
        }

        public async Task<bool> UpdateStatusBooking(Booking booking)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                _dbContext.Bookings.Update(booking);
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

        public async Task<IEnumerable<Booking>> SearchConfirmBooked()
        {
            return await _dbContext.Bookings.Include(x => x.BookingDetails)
                .Include(x => x.Customer).Where(x => x.BookingStatus == 2).ToListAsync();
        }

        public async Task<IEnumerable<Booking>> SearchConfirmChecked()
        {
            return await _dbContext.Bookings.Include(x => x.BookingDetails)
                .Include(x => x.Customer).Where(x => x.BookingStatus == 3).ToListAsync();
        }

        public async Task<IEnumerable<Booking>> SearchExit(DateTime checkIn, DateTime checkOut, string CustomerName, string PhoneNumber)
        {
            var result=  await _dbContext.Bookings.Include(x => x.BookingDetails)
                .Include(x => x.Customer).Where(x => x.BookingStatus == 4)
                .Where(x=>x.CheckInTime>=checkIn)
                .Where(x=>x.CheckOutTime<=checkOut)
                .Where(x=>x.Customer.CustomerName.Trim().ToLower().Contains(string.IsNullOrEmpty(CustomerName)? string.Empty: CustomerName))
                .Where(x=>x.Customer.PhoneNumber.Trim().ToLower().Contains(string.IsNullOrEmpty(PhoneNumber) ? string.Empty: PhoneNumber))
                .ToListAsync();
            
            return result;
        }

        public async Task<bool> Delete(Booking bookingDetails)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                _dbContext.Bookings.Remove(bookingDetails);
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

        public async Task<decimal> StatisticsMonthByYear(int Month, int Year)
        {
            DateTime startDay = new DateTime(Year, Month, 1);
            DateTime endDay = startDay.AddMonths(1).AddDays(-1);
            var Count = await _dbContext.Bookings.Where(x => x.CheckInTime >= startDay && x.CheckInTime <= endDay
            && x.BookingStatus==4).Select(x => x.SumCost).ToListAsync();
            decimal sum = 0;
            foreach (var item in Count)
            {
                sum += item;
            }
            return sum;
        }

        public async Task<decimal> GetCostRoomTypes(int roomTypeId, int Year)
        {
            DateTime startDay = new DateTime(Year, 1, 1);
            DateTime endDay = new DateTime(Year, 12, 31);
            var rooms = await _dbContext.Rooms.Where(x => x.RoomTypeId == roomTypeId).Select(x=>x.RoomId).ToListAsync();
            var bookingDetails = await _dbContext.BookingDetails.Where(x => rooms.Contains(x.RoomId) && x.Booking.BookingStatus == 4 && x.Booking.CheckInTime >= startDay && x.Booking.CheckInTime <= endDay).Select(x=>x.CostNow * ((x.Booking.CheckOutTime - x.Booking.CheckInTime).Days + 1)).ToListAsync();

            return bookingDetails.Sum();
        }
        public async Task<decimal> GetCostRoomTypesByMonth(int roomTypeId, int Month, int Year)
        {
            DateTime startDay = new DateTime(Year, Month, 1);
            DateTime endDay = startDay.AddMonths(1).AddDays(-1);
            var rooms = await _dbContext.Rooms.Where(x => x.RoomTypeId == roomTypeId).Select(x => x.RoomId).ToListAsync();
            var bookingDetails = await _dbContext.BookingDetails.Include(x=>x.Booking).Where(x => rooms.Contains(x.RoomId) && x.Booking.CheckInTime >= startDay && x.Booking.CheckInTime <= endDay && x.Booking.BookingStatus==4).Select(x => x.CostNow*((x.Booking.CheckOutTime-x.Booking.CheckInTime).Days +1)).ToListAsync();
            return bookingDetails.Sum();
        }
    }
}
