using HoangNV.HotelBooking.BusinessLogic.Models;
using HoangNV.HotelBooking.Core.Services;
using HoangNV.HotelBooking.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.BusinessLogic.Interface
{
    public interface IRoomTypeBS : IBaseBS<RoomTypes>
    {
        Task<IEnumerable<RoomTypeViewModel>> Search(RoomTypeSearchModel roomTypeViewModel);
        Task<IEnumerable<RoomTypeViewModel>> SearchClient(RoomTypeSearchModel roomTypeViewModel);
        Task<RoomTypeUpdteVM> GetByIdInclude(int id);
        Task<bool> Insert(RoomTypeViewModel roomTypeViewModel);
        Task<bool> Update(RoomTypeUpdteVM roomTypeUpdateViewModel);
        Task<bool> Delete(int id);
    }
}
