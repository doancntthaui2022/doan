using HoangNV.HotelBooking.Core.Repository;
using HoangNV.HotelBooking.Entities.Entities;
using HoangNV.HotelBooking.Repository.Enum;
using HoangNV.HotelBooking.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Repository
{
    public class UserRepository :Repository<Users>, IUserRepository
    {
        private readonly BookingContext _dbContext;
        public UserRepository(BookingContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public (Users,string) GetUser(string userName, string passWord)
        {
            var user= _dbContext.Users
                .Include(r => r.Roles)
                .FirstOrDefault(x => x.UserName == userName && x.Active == 1);
            if (user != null)
            {
                PasswordHasher<Users> passwordHasher = new PasswordHasher<Users>();
                PasswordVerificationResult result = passwordHasher.VerifyHashedPassword(user,user.PassWord, passWord);
                if(result!=PasswordVerificationResult.Failed)
                {
                    var role = user.Roles.RoleName;
                    return (user,role);
                }    
            }
            return (null,null);
        }
        public async Task<IEnumerable<Users>> GetList(string userName, string fullName, int role)
        {
            var user = _dbContext.Users.Include(x=>x.Roles)
                .Where(x=>x.Active==1)
                .Where(x => x.UserName.Trim().ToLower().Contains(string.IsNullOrEmpty(userName) ? string.Empty : userName.Trim().ToLower()))
                .Where(x => x.FullName.Trim().ToLower().Contains(string.IsNullOrEmpty(fullName) ? string.Empty : fullName.Trim().ToLower()));
            if (role != (int)RoleEnum.All)
            {
                string roleString = System.Enum.GetName(typeof(RoleEnum), role);
                var roles = await _dbContext.Roles.FirstOrDefaultAsync(x => x.RoleName == roleString);
                user = user.Where(x => x.RoleId == roles.RoleId);
            }
            else
            {
                user = user.Where(x => x.Roles.RoleName != "Admin");
            }
            return user.ToList();
        }

        public async Task<IEnumerable<Users>> GetListAll()
        {
            return await _dbContext.Users.Include(x => x.Roles).Where(x => x.Active == 1).ToListAsync();
        }
        public async Task<Users> GetUserByName(string userName)
        {
            return await _dbContext.Users.Include(x => x.Roles).FirstOrDefaultAsync(x => x.Active == 1 && userName==x.UserName);
        }
        public async Task<bool> Insert(Users users)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await _dbContext.Users.AddAsync(users);
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
        public async Task<bool> Update(Users users)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                _dbContext.Users.Update(users);
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
