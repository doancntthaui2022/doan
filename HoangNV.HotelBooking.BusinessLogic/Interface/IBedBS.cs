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
    public interface IBedBS : IBaseBS<Beds>
    {
        Task<IEnumerable<BedQueryModel>> Search(string nameBedType);
        Task<bool> Insert(string nameBedType);
        Task<bool> Delete(int id);
        Task<bool> Update(BedViewModel bedVM);
    }
}
