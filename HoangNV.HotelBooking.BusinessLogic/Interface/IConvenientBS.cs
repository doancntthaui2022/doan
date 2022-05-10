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
    public interface IConvenientBS : IBaseBS<Convenients>
    {
        Task<IEnumerable<ConvenientQueryModel>> Search(ConvenientSearchModel convenientSearchModel);
        Task<ConvenientQueryModel> GetConvenientQueryModelsByName(string name);
        Task<bool> Insert(ConvenientViewModel convenientSearchModel);
        Task<bool> Update(ConvenientViewModel convenientSearchModel);
        Task<bool> Delete(int id);
    }
}
