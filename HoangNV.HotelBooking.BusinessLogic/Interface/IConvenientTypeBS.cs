using HoangNV.HotelBooking.Core.Services;
using HoangNV.HotelBooking.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.BusinessLogic.Interface
{
    public interface IConvenientTypeBS :IBaseBS<ConvenientTypes>
    {
        Task<IEnumerable<ConvenientTypes>> Search(string searchName);
        Task<IEnumerable<ConvenientTypes>> SearchWithConvenient(string searchName);
        Task<ConvenientTypes> SearchByName(string searchName);

        Task<bool> Insert(string convenientTypeName);
        Task<bool> Delete(int id);
        Task<bool> Update(int id, string newName);
    }
}
