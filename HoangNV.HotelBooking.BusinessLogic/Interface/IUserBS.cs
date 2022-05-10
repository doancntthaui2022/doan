using HoangNV.HotelBooking.BusinessLogic.Models;
using HoangNV.HotelBooking.Core.Services;
using HoangNV.HotelBooking.Entities.Entities;
using HoangNV.HotelBooking.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.BusinessLogic.Interface
{
    public interface IUserBS : IBaseBS<Users>
    {
        UserViewModel GetUsers(string userName, string passWord);

        Task<IEnumerable<UserResultModel>> Search(UserSearchModel model);
        Task<IEnumerable<UserResultModel>> SearchAll();
        Task<UserUpdateViewModel> GetUserByName(string userName);
        Task<PassWordUpdateViewModel> GetUserUpdatePassByName(string userName);
        Task<bool> Insert(UserAddViewModel model);
        
        Task<bool> UpdateUser(string id);
        Task<bool> UpdateMyUser(UserUpdateViewModel model);
        Task<bool> UpdateMyPass(PassWordUpdateViewModel model);
    }
}
