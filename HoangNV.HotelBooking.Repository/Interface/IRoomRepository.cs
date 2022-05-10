using HoangNV.HotelBooking.Core.Repository.Interface;
using HoangNV.HotelBooking.Entities.Entities;
using HoangNV.HotelBooking.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Repository.Interface
{
    public interface IRoomRepository : IRepository<Rooms>
    {
        Task<IEnumerable<RoomQueryModel>> Search(string roomCode, int RoomTypeId, int Status);
        Task<IEnumerable<Rooms>> SearchInclue(string roomCode, int RoomTypeId, int Status);
        Task<bool> Insert(Rooms rooms);
        Task<bool> Update(Rooms rooms);
        Task<bool> Delete(Rooms rooms);

        Task<IEnumerable<Rooms>> GetRoomsByRoomTypeId(int roomTypeId);
    }
}
