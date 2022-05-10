using HoangNV.HotelBooking.Core.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Core.Services
{
    public interface IBaseBS<T> where T : BaseEntity
    {
        [Obsolete("This property is obsolete. Use DBSetExtensions instead for all query.", false)]
        T GetById(int id);

        [Obsolete("This property is obsolete. Use DBSetExtensions instead for all query.", false)]
        IReadOnlyList<T> ListAll();

        T Add(T entity, bool isSave = true);

        void Update(T entity, bool isSave = true);

        void Delete(T entity, bool isSave = true);

        void SaveChanges();

        IDbContextTransaction BeginTransaction();

        [Obsolete("This property is obsolete. Use DBSetExtensions instead for all query.", false)]
        Task<T> GetByIdAsync(params object[] keyValues);

        [Obsolete("This property is obsolete. Use DBSetExtensions instead for all query.", false)]
        Task<IReadOnlyList<T>> ListAllAsync();

        Task<T> AddAsync(T entity, bool saveChange = true);

        Task UpdateAsync(T entity, bool saveChange = true);

        Task DeleteAsync(T entity, bool saveChange = true);

        Task SaveChangesAsync();

        Task<IDbContextTransaction> BeginTransactionAsync();

    }
}
