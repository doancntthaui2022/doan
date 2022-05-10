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
    public interface IHotelBranchRepository : IRepository<HotelBranchs>
    {
        //Task<IEnumerable<HotelBranchs>> Search();
        //Task<bool> Insert(HotelBranchs hotelBranchs);
        //Task<HotelBranchs> SearchById(int id);
        //Task<bool> Delete(int id);

        //Task<bool> UpdateStatus(int id);
        Task<bool> UpdateHotelBranch(HotelBranchs hotel);

        Task<HotelBranchs> GetFristOrDefault();
    }
}
