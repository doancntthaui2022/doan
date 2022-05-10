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
    public class RoomBedRepository : Repository<RoomBeds>, IRoomBedRepository
    {
        private readonly BookingContext _dbContext;
        public RoomBedRepository(BookingContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Insert(RoomBeds RoomBeds)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await _dbContext.RoomBeds.AddAsync(RoomBeds);
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
        public async Task<IEnumerable<RoomBeds>> GetRoomBedsByRoomTypeId(int roomTypeId)
        {
            return await _dbContext.RoomBeds.Include(x=>x.Bed).Where(x => x.RoomTypeId == roomTypeId).ToListAsync();
        }

        public async Task<bool> Delete(RoomBeds RoomBeds)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                _dbContext.RoomBeds.Remove(RoomBeds);
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
