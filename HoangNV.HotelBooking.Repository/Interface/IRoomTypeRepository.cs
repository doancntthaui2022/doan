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
    public interface IRoomTypeRepository : IRepository<RoomTypes>
    {
        Task<IEnumerable<RoomTypes>> Search(string roomTypeCode, string roomTypeName);
        Task<RoomTypes> GetByCode(string roomTypeCode);
        Task<RoomTypes> GetByIdInclude(int id);
        Task<bool> Insert(RoomTypes roomTypes);
        Task<bool> Update(RoomTypes roomTypes);
        Task<bool> Delete(RoomTypes roomTypes);
    }
}
