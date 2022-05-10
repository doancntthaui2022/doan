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
    public class ConvenientTypeRepository : Repository<ConvenientTypes>, IConvenientTypeRepository
    {
        private readonly BookingContext _dbContext;
        public ConvenientTypeRepository(BookingContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ConvenientTypes>> Search(string nameConvenientType)
        {
            return await _dbContext.ConvenientTypes.Where(x => x.ConvenientTypeName.ToLower().Contains(String.IsNullOrEmpty(nameConvenientType) ? "" : nameConvenientType.ToLower().Trim())).OrderByDescending(x=>x.ConvenientTypeId).ToListAsync();
        }

        public async Task<IEnumerable<ConvenientTypes>> SearchWithConvenient(string nameConvenientType)
        {
            return await _dbContext.ConvenientTypes.Include(x => x.Convenients).Where(x => x.ConvenientTypeName.ToLower().Contains(String.IsNullOrEmpty(nameConvenientType) ? "" : nameConvenientType.ToLower().Trim())).ToListAsync();
        }
        public async Task<ConvenientTypes> SearchByName(string nameConvenientType)
        {
            return await _dbContext.ConvenientTypes.FirstOrDefaultAsync(x => x.ConvenientTypeName.ToLower()== nameConvenientType.ToLower());
        }
        public async Task<bool> Insert(ConvenientTypes convenientType)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await _dbContext.ConvenientTypes.AddAsync(convenientType);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch(Exception)
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
                var convenientType = await _dbContext.ConvenientTypes.FindAsync(id);
                _dbContext.ConvenientTypes.Remove(convenientType);
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
                var convenientType = await _dbContext.ConvenientTypes.FindAsync(id);
                convenientType.ConvenientTypeName = newName;
                _dbContext.ConvenientTypes.Update(convenientType);
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
