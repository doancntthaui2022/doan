using HoangNV.HotelBooking.BusinessLogic.Interface;
using HoangNV.HotelBooking.BusinessLogic.Models;
using HoangNV.HotelBooking.Core.Services;
using HoangNV.HotelBooking.Entities.Entities;
using HoangNV.HotelBooking.Repository.Interface;
using HoangNV.HotelBooking.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.BusinessLogic
{
    public class BedBS : BaseBS<Beds>, IBedBS
    {
        public IBedRepository _repository;
        public BedBS(IBedRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<BedQueryModel>> Search(string searchName)
        {
            return await _repository.Search(searchName);
        }

        public async Task<bool> Insert(string nameBedType)
        {

            var beds = new Beds()
            {
                BedType = nameBedType
            };

            return await _repository.Insert(beds);
        }

        public async Task<bool> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<bool> Update(BedViewModel bedVM)
        {
            return await _repository.Update(bedVM.BedId,bedVM.BedType);
        }
    }
}
