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
    public class ConvenientRepository : Repository<Convenients>, IConvenientRepository
    {
        private readonly BookingContext _dbContext;
        public ConvenientRepository(BookingContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ConvenientQueryModel>> Search(string nameConvenient, int convientTypeId)
        {
            var query = _dbContext.Convenients
                    .Include(r => r.ConvenientType)
                    .Where(x => x.ConvenientName.ToLower().Contains(String.IsNullOrEmpty(nameConvenient) ? "" : nameConvenient.Trim().ToLower()));
            if (convientTypeId != 0)
                query = query.Where(x => x.ConvenientTypeId == convientTypeId);
            return await query.Select(p => new ConvenientQueryModel()
            {
                ConvenientId = p.ConvenientId,
                ConvenientName = p.ConvenientName,
                ConvenientTypeName = p.ConvenientType.ConvenientTypeName,
                ConvenientTypeId=p.ConvenientType.ConvenientTypeId,
            }).OrderByDescending(x=>x.ConvenientId).ToListAsync();
        }

        public async Task<ConvenientQueryModel> GetConvenientQueryModelsByName(string nameConvenient)
        {
            var query = await _dbContext.Convenients
                     .Include(r => r.ConvenientType)
                     .FirstOrDefaultAsync(x => x.ConvenientName.ToLower()== nameConvenient.Trim().ToLower());
            if (query != null)
                return new ConvenientQueryModel()
                {
                    ConvenientId = query.ConvenientId,
                    ConvenientName = query.ConvenientName,
                    ConvenientTypeName = query.ConvenientType.ConvenientTypeName,
                };
            else return null;
        }
        
        public async Task<bool> Insert(string convenientName, int convenientTypeId)
        {
            Convenients convenients = new Convenients()
            {
                ConvenientName = convenientName,
                ConvenientTypeId = convenientTypeId
            };
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await _dbContext.Convenients.AddAsync(convenients);
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
                var convenient = await _dbContext.Convenients.FindAsync(id);
                _dbContext.Convenients.Remove(convenient);
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

        public async Task<bool> Update(int convenientId, string convenientName, int convenientTypeId)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var convenient = await _dbContext.Convenients.FindAsync(convenientId);
                convenient.ConvenientName = convenientName;
                convenient.ConvenientTypeId = convenientTypeId;
                _dbContext.Convenients.Update(convenient);
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
