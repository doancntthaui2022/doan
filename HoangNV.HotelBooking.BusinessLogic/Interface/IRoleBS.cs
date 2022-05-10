using HoangNV.HotelBooking.Core.Services;
using HoangNV.HotelBooking.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.BusinessLogic.Interface
{
    public interface IRoleBS : IBaseBS<Roles>
    {
        Task<IEnumerable<Roles>> GetList();
    }
}
