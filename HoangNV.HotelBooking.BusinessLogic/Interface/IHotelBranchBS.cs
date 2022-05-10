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
    public interface IHotelBranchBS : IBaseBS<HotelBranchs>
    {
        //Task<IEnumerable<HotelBranchQueryModel>> Search(HotelBranchSearchModel branchSearchModel);
        //Task<bool> Insert(HotelBranchViewModel hotelVM);
        Task<bool> Update(HotelBranchViewModel hotelVM);

        Task<HotelBranchViewModel> GetFristOrDefault();

        //Task<HotelBranchViewModel> SearchById(int id);
        //Task<bool> Delete(int id);
    }
}
