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
    public class ImageRepository : Repository<Images>, IImageRepository
    {
        private readonly BookingContext _dbContext;
        public ImageRepository(BookingContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Images> Insert(Images images)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await _dbContext.Images.AddAsync(images);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return null;
            }
            return images;
        }

        public async Task<bool> Delete(Images images)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                _dbContext.Images.Remove(images);
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

        public IEnumerable<Images> GetImagesByHotelBranchId(int hotelBranchId)
        {
            return _dbContext.Images.Where(x => x.HotelBranchId == hotelBranchId).ToList();
        }

        public async Task<IEnumerable<Images>> GetImagesByRoomTypeId(int roomTypeId)
        {
            return await _dbContext.Images.Where(x => x.RoomTypeId == roomTypeId).ToListAsync();
        }
    }
}
