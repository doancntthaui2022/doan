

using HoangNV.HotelBooking.Core.Repository.Interface;
using HoangNV.HotelBooking.Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Repository.Interface
{
    public interface IUserRepository : IRepository<Users>
    {
        (Users, string) GetUser(string userName, string passWord);
        Task<IEnumerable<Users>> GetList(string userName,string fullName,int role);
        Task<IEnumerable<Users>> GetListAll();
        Task<Users> GetUserByName(string userName);
        Task<bool> Insert(Users users);
        Task<bool> Update(Users users);
    }
}
