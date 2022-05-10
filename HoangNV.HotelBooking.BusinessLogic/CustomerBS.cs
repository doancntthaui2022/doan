using HoangNV.HotelBooking.BusinessLogic.Interface;
using HoangNV.HotelBooking.Core.Services;
using HoangNV.HotelBooking.Entities.Entities;
using HoangNV.HotelBooking.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.BusinessLogic
{
    public class CustomerBS : BaseBS<Customers>, ICustomerBS
    {
        public ICustomerRepository _repository;
        public CustomerBS(ICustomerRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<Customers> GetCustomerByUserName(string name)
        {
            return await _repository.GetCustomerByUserName(name);
        }
    }
}
