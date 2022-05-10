using HoangNV.HotelBooking.BusinessLogic.Interface;
using HoangNV.HotelBooking.Core.Repository.Interface;
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
    public class ConvenientTypeBS : BaseBS<ConvenientTypes>, IConvenientTypeBS
    {
        public IConvenientTypeRepository _repository;
        public ConvenientTypeBS(IConvenientTypeRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ConvenientTypes>> Search(string searchName)
        {
            return await _repository.Search(searchName);
        }
        public async Task<IEnumerable<ConvenientTypes>> SearchWithConvenient(string searchName)
        {
            return await _repository.SearchWithConvenient(searchName);
        }
        public async Task<ConvenientTypes> SearchByName(string searchName)
        {
            return await _repository.SearchByName(searchName);
        }
        public async Task<bool> Insert(string convenientTypeName)
        {
            
            var convenientType = new ConvenientTypes()
            {
                ConvenientTypeName = convenientTypeName
            };

            return await _repository.Insert(convenientType);
        }

        public async Task<bool> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<bool> Update(int id, string newName)
        {
            return await _repository.Update(id,newName);
        }
    }
}
