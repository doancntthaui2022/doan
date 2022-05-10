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
    public interface IConvenientRepository : IRepository<Convenients>
    {
        Task<IEnumerable<ConvenientQueryModel>> Search(string nameConvenient, int convientTypeId);
        Task<ConvenientQueryModel> GetConvenientQueryModelsByName(string nameConvenient);
        Task<bool> Insert(string ConvenientName, int ConvenientTypeId);
        Task<bool> Update(int ConvenientId,string ConvenientName, int ConvenientTypeId);
        Task<bool> Delete(int id);
    }
}
