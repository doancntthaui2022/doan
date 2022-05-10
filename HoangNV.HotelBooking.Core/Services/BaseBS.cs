using HoangNV.HotelBooking.Core.Entities;
using HoangNV.HotelBooking.Core.Repository.Interface;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Core.Services
{
    public class BaseBS<T> : IBaseBS<T> where T : BaseEntity
    {
        private readonly IRepository<T> _repository;

        public BaseBS(IRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual T GetById(int id)
        {
            return _repository.GetById(id);
        }

        public virtual IReadOnlyList<T> ListAll()
        {
            return _repository.ListAll();
        }

        public T Add(T entity, bool isSave = true)
        {
            return _repository.Add(entity, isSave);
        }

        public void Update(T entity, bool isSave = true)
        {
            _repository.Update(entity, isSave);
        }

        public void Delete(T entity, bool isSave = true)
        {
            _repository.Delete(entity, isSave);
        }

        public void SaveChanges()
        {
            _repository.SaveChanges();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _repository.BeginTransaction();
        }

        public virtual async Task<T> GetByIdAsync(params object[] keyValues)
        {
            return await _repository.GetByIdAsync(keyValues);
        }

        public virtual async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _repository.ListAllAsync();
        }

        public async Task<T> AddAsync(T entity, bool saveChange = true)
        {
            return await _repository.AddAsync(entity, saveChange);
        }

        public async Task UpdateAsync(T entity, bool saveChange = true)
        {
            await _repository.UpdateAsync(entity, saveChange);
        }

        public async Task DeleteAsync(T entity, bool saveChange = true)
        {
            await _repository.DeleteAsync(entity, saveChange);
        }

        public async Task SaveChangesAsync()
        {
            await _repository.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _repository.BeginTransactionAsync();
        }
    }
}
