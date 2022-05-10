using HoangNV.HotelBooking.Core.Repository;
using HoangNV.HotelBooking.Entities.Entities;
using HoangNV.HotelBooking.Repository.Enum;
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
    public class RoomRepository : Repository<Rooms>, IRoomRepository
    {
        private readonly BookingContext _dbContext;
        public RoomRepository(BookingContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Rooms>> GetRoomsByRoomTypeId(int roomTypeId)
        {
            return await _dbContext.Rooms.Where(x => x.RoomTypeId == roomTypeId).OrderByDescending(x=>x.RoomCode).ToListAsync();
        }
        public async Task<IEnumerable<RoomQueryModel>> Search(string roomCode, int RoomTypeId, int Status)
        {
            var query = _dbContext.Rooms.Where(x => x.RoomCode.Trim().ToLower().Contains((string.IsNullOrEmpty(roomCode) ? string.Empty : roomCode.Trim().ToLower())));
            if (RoomTypeId != 0)
                query = query.Where(x => x.RoomTypeId == RoomTypeId);
            if (Status != (int)RoomEnum.All)
                query = query.Where(x => x.Status == Status);
            return await query.Select(x => new RoomQueryModel()
            {
                RoomTypeId = x.RoomTypeId,
                RoomCode = x.RoomCode,
                RoomId = x.RoomId,
                Status = (int)x.Status,
            }).OrderByDescending(x=>x.RoomId).ToListAsync();
        }
        public async Task<IEnumerable<Rooms>> SearchInclue(string roomCode, int RoomTypeId, int Status)
        {
            var query = _dbContext.Rooms
                .Include(x=>x.RoomType)
                .Where(x => x.RoomCode.Trim().ToLower().Contains((string.IsNullOrEmpty(roomCode) ? string.Empty : roomCode.Trim().ToLower())));
            if (RoomTypeId != 0)
                query = query.Where(x => x.RoomTypeId == RoomTypeId);
            if (Status != (int)RoomEnum.All)
                query = query.Where(x => x.Status == Status);
            return await query.ToListAsync();
        }
        public async Task<bool> Insert(Rooms rooms)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await _dbContext.Rooms.AddAsync(rooms);
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
        public async Task<bool> Delete(Rooms rooms)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                _dbContext.Rooms.Remove(rooms);
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

        public async Task<bool> Update(Rooms rooms)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                _dbContext.Rooms.Update(rooms);
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
