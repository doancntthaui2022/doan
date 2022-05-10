using HoangNV.HotelBooking.Core.Repository;
using HoangNV.HotelBooking.Entities.Entities;
using HoangNV.HotelBooking.Repository.Interface;
using HoangNV.HotelBooking.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Repository
{
    public class RoomTypeRepository : Repository<RoomTypes>, IRoomTypeRepository
    {
        private readonly BookingContext _dbContext;
        public RoomTypeRepository(BookingContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RoomTypes> GetByCode(string roomTypeCode)
        {
            return await _dbContext.RoomTypes.FirstOrDefaultAsync(x => x.RoomTypeCode == roomTypeCode);
        }
        public async Task<IEnumerable<RoomTypes>> Search(string roomTypeCode,string roomTypeName)
        {
            return await _dbContext.RoomTypes
                .Include(x=>x.Images)
                .Include(x=>x.HotelBranch)
                .Include(x=>x.RoomBeds)
                .Include(x=>x.Rooms)
                .Include(x=>x.RoomConvenients)
                .Where(x => x.RoomTypeCode.Trim().ToLower().Contains((string.IsNullOrEmpty(roomTypeCode) ? string.Empty : roomTypeCode.Trim().ToLower()))).
                Where(x => x.RoomTypeName.Trim().ToLower().Contains((string.IsNullOrEmpty(roomTypeName) ? string.Empty : roomTypeName.Trim().ToLower()))).OrderByDescending(x=>x.RoomTypeId).ToListAsync();
        }
        public async Task<RoomTypes> GetByIdInclude(int id)
        {
            return await _dbContext.RoomTypes
                .Include(x => x.Images)
                .Include(x => x.HotelBranch)
                .Include(x => x.RoomBeds)
                .Include(x => x.Rooms)
                .Include(x => x.RoomConvenients)
                .FirstOrDefaultAsync(x => x.RoomTypeId==id);
        }
        public async Task<bool> Insert(RoomTypes roomTypes)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await _dbContext.RoomTypes.AddAsync(roomTypes);
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
        public async Task<bool> Delete(RoomTypes roomTypes)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                _dbContext.RoomTypes.Remove(roomTypes);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
            }
            return true;
        }

        public async Task<bool> Update(RoomTypes roomTypes)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var entry = _dbContext.RoomTypes.First(e => e.RoomTypeId == roomTypes.RoomTypeId);
                _dbContext.Entry(entry).CurrentValues.SetValues(roomTypes);
                //_dbContext.RoomTypes.Update(roomTypes);
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
