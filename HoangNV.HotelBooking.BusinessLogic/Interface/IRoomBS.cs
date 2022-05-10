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
    public interface IRoomBS : IBaseBS<Rooms>
    {
        Task<IEnumerable<RoomViewModel>> Search(RoomViewModel searchCondition);

        Task<string> UpdateStatus(int id, int status);
        Task<bool> Delete(int id);

        Task<int> GetRoomNameMax(int roomTypeId);
        Task<bool> Insert(RoomAddViewModel roomAddViewModel);
    }
}
