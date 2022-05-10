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
    public class BedRepository : Repository<Beds>, IBedRepository
    {
        private readonly BookingContext _dbContext;
        public BedRepository(BookingContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<BedQueryModel>> Search(string nameBedType)
        {
            var query = _dbContext.Beds
                    .Where(x => x.BedType.ToLower().Trim().Contains(String.IsNullOrEmpty(nameBedType) ? "" : nameBedType.Trim().ToLower()));
            return await query.Select(p => new BedQueryModel()
            {
                BedId=p.BedId,
                BedType=p.BedType
            }).OrderByDescending(x=>x.BedId).ToListAsync();
        }

        public async Task<bool> Insert(Beds beds)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await _dbContext.Beds.AddAsync(beds);
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

        public async Task<bool> Delete(int id)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var beds = await _dbContext.Beds.FindAsync(id);
                _dbContext.Beds.Remove(beds);
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

        public async Task<bool> Update(int id, string newName)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var beds = await _dbContext.Beds.FindAsync(id);
                beds.BedType = newName;
                _dbContext.Beds.Update(beds);
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
