using HoangNV.HotelBooking.Core.Repository.Interface;
using HoangNV.HotelBooking.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Repository.Interface
{
    public interface IImageRepository : IRepository<Images>
    {
        Task<Images> Insert(Images images);
        Task<bool> Delete(Images images);
        IEnumerable<Images> GetImagesByHotelBranchId(int hotelBranchId);
        Task<IEnumerable<Images>> GetImagesByRoomTypeId(int roomTypeId);
    }
}
