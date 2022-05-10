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
    public class RoleRepository : Repository<Roles>, IRoleRepository
    {
        private readonly BookingContext _dbContext;
        public RoleRepository(BookingContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Roles>> GetList()
        {
            return await _dbContext.Roles.ToListAsync();
        }

        public async Task<Roles> GetRoleByName(string roleName)
        {
            return await _dbContext.Roles.FirstOrDefaultAsync(x => x.RoleName == roleName);
        }
    }
}
