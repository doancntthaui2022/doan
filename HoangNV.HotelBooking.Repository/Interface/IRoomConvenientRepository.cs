using HoangNV.HotelBooking.Core.Repository.Interface;
using HoangNV.HotelBooking.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Repository.Interface
{
    public interface IRoomConvenientRepository : IRepository<RoomConvenients>
    {
        Task<bool> Insert(RoomConvenients roomConvenients);
        Task<bool> Delete(RoomConvenients roomConvenients);

        Task<IEnumerable<RoomConvenients>> GetRoomConvenientsByRoomTypeId(int roomTypeId);
    }
}
