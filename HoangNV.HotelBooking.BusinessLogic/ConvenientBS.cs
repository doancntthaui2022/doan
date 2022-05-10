using HoangNV.HotelBooking.BusinessLogic.Interface;
using HoangNV.HotelBooking.BusinessLogic.Models;
using HoangNV.HotelBooking.Core.Repository.Interface;
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
    public class ConvenientBS : BaseBS<Convenients>, IConvenientBS
    {
        public IConvenientRepository _repository;
        public IConvenientTypeRepository _convenientTypeRepository;
        public ConvenientBS(IConvenientRepository repository, IConvenientTypeRepository convenientTypeRepository) : base(repository)
        {
            _repository = repository;
            _convenientTypeRepository = convenientTypeRepository;
        }

        public async Task<IEnumerable<ConvenientQueryModel>> Search(ConvenientSearchModel convenientSearchModel)
        {
            return await _repository.Search(convenientSearchModel.ConvenientName, convenientSearchModel.ConvenientTypeId);
        }

        public async Task<ConvenientQueryModel> GetConvenientQueryModelsByName(string name)
        {
            return await _repository.GetConvenientQueryModelsByName(name);
        }

        public async Task<bool> Insert(ConvenientViewModel convenientSearchModel)
        {
            return await _repository.Insert(convenientSearchModel.ConvenientName, convenientSearchModel.ConvenientTypeId);
        }

        public async Task<bool> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<bool> Update(ConvenientViewModel convenientSearchModel)
        {
            return await _repository.Update(convenientSearchModel.ConvenientId, convenientSearchModel.ConvenientName, convenientSearchModel.ConvenientTypeId);
        }
    }
}
