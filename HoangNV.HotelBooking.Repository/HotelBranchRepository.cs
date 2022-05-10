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
    public class HotelBranchRepository : Repository<HotelBranchs>, IHotelBranchRepository
    {
        private readonly BookingContext _dbContext;
        public HotelBranchRepository(BookingContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        //public async Task<HotelBranchs> SearchById(int id)
        //{
        //    var query = await _dbContext.HotelBranchs.Include(x=>x.Images).FirstOrDefaultAsync(x=>x.HotelBranchId==id);
        //    return query;
        //}
        //public async Task<IEnumerable<HotelBranchs>> Search()
        //{
        //    var query = await _dbContext.HotelBranchs
        //                    .Include(x=>x.Images)
        //                    .OrderByDescending(x=>x.HotelBranchId).OrderBy(x=>x.Status).ToListAsync();
        //    return query;
        //}

        //public async Task<bool> Insert(HotelBranchs hotelBranchs)
        //{
        //    await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        //    try
        //    {
        //        await _dbContext.HotelBranchs.AddAsync(hotelBranchs);
        //        await _dbContext.SaveChangesAsync();
        //        await transaction.CommitAsync();
        //    }
        //    catch (Exception)
        //    {
        //        await transaction.RollbackAsync();
        //        return false;
        //    }
        //    return true;
        //}

        //public async Task<bool> Delete(int id)
        //{
        //    await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        //    try
        //    {
        //        var hotel = await _dbContext.HotelBranchs.FindAsync(id);
        //        _dbContext.HotelBranchs.Remove(hotel);
        //        await _dbContext.SaveChangesAsync();
        //        await transaction.CommitAsync();
        //    }
        //    catch (Exception)
        //    {
        //        await transaction.RollbackAsync();
        //        return false;
        //    }
        //    return true;
        //}

        //public async Task<bool> UpdateStatus(int id)
        //{
        //    await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        //    try
        //    {
        //        var hotel = await _dbContext.HotelBranchs.FindAsync(id);
        //        hotel.Status = (int)HotelEnum.NonActived;
        //        _dbContext.HotelBranchs.Update(hotel);
        //        await _dbContext.SaveChangesAsync();
        //        await transaction.CommitAsync();
        //    }
        //    catch (Exception)
        //    {
        //        await transaction.RollbackAsync();
        //        return false;
        //    }
        //    return true;
        //}

        public async Task<bool> UpdateHotelBranch(HotelBranchs hotel)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                _dbContext.HotelBranchs.Update(hotel);
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

        public async Task<HotelBranchs> GetFristOrDefault()
        {
            return (await _dbContext.HotelBranchs.Include(x=>x.Images).ToListAsync()).FirstOrDefault();
        }
    }
}
