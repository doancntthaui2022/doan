using HoangNV.HotelBooking.Core.Repository.Interface;
using HoangNV.HotelBooking.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Repository.Interface
{
    public interface IConvenientTypeRepository :IRepository<ConvenientTypes>
    {
        Task<IEnumerable<ConvenientTypes>> Search(string nameConvenientType);
        Task<IEnumerable<ConvenientTypes>> SearchWithConvenient(string nameConvenientType);
        Task<ConvenientTypes> SearchByName(string nameConvenientType);

        Task<bool> Insert(ConvenientTypes ConvenientType);
        Task<bool> Delete(int id);
        Task<bool> Update(int id,string newName);
    }
}
