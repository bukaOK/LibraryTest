using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTest.DAL.Core
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetAsync(params object[] keyValues);
        /// <summary>
        /// Возвращает список всех сущностей данного типа
        /// </summary>
        /// <returns></returns>
        Task<IList<TEntity>> GetAllAsync();
        /// <summary>
        /// Добавляет сущность
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task AddAsync(TEntity entity);
        Task RemoveAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task<int> GetCountAsync();

        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
