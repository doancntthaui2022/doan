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
    public class CustomerRepository: Repository<Customers>, ICustomerRepository
    {
        private readonly BookingContext _dbContext;
        public CustomerRepository(BookingContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Insert(Customers customers)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await _dbContext.Customers.AddAsync(customers);
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
        public async Task<Customers> GetCustomerByUserName(string name)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x=>x.UserName== name);
            if (user.CustomerId == null) return null;
            var customer = await _dbContext.Customers.Include(x => x.Bookings).FirstOrDefaultAsync(x => x.CustomerId == user.CustomerId);
            return customer;
        }

        public async Task<bool> Update(Customers customers)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                _dbContext.Customers.Update(customers);
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
