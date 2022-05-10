using HoangNV.HotelBooking.Core.Repository.Interface;
using HoangNV.HotelBooking.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Repository.Interface
{
    public interface ICustomerRepository : IRepository<Customers>
    {
        Task<bool> Insert(Customers customers);
        Task<bool> Update(Customers customers);
        Task<Customers> GetCustomerByUserName(string name);
    }
}
