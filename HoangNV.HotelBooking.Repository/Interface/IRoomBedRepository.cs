using HoangNV.HotelBooking.Core.Repository.Interface;
using HoangNV.HotelBooking.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Repository.Interface
{
    public interface IRoomBedRepository : IRepository<RoomBeds>
    {
        Task<bool> Insert(RoomBeds RoomBeds);
        Task<bool> Delete(RoomBeds RoomBeds);

        Task<IEnumerable<RoomBeds>> GetRoomBedsByRoomTypeId(int roomTypeId);
    }
}
