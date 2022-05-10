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
    public interface IBedRepository : IRepository<Beds>
    {
        Task<IEnumerable<BedQueryModel>> Search(string nameBedType);
        Task<bool> Insert(Beds beds);
        Task<bool> Update(int bedId, string bedType);
        Task<bool> Delete(int id);
    }
}
