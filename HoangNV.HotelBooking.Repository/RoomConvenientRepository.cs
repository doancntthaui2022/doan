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
    public class RoomConvenientRepository : Repository<RoomConvenients>, IRoomConvenientRepository
    {
        private readonly BookingContext _dbContext;
        public RoomConvenientRepository(BookingContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Insert(RoomConvenients roomConvenients)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await _dbContext.RoomConvenients.AddAsync(roomConvenients);
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

        public async Task<IEnumerable<RoomConvenients>> GetRoomConvenientsByRoomTypeId(int roomTypeId)
        {
            return await _dbContext.RoomConvenients.Include(x=>x.Convenient).Where(x => x.RoomTypeId == roomTypeId).ToListAsync();
        }

        public async Task<bool> Delete(RoomConvenients roomConvenients)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                _dbContext.RoomConvenients.Remove(roomConvenients);
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
    }
}
