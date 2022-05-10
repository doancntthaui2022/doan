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
    public class RoleBS : BaseBS<Roles>, IRoleBS
    {
        public IRoleRepository _repository;
        public RoleBS(IRoleRepository repository) : base(repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<Roles>> GetList()
        {
            return await _repository.GetList();
        }
    }
}
