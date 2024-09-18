using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VoteApp.Domain
{
    public interface IBaseRepository<T>
    {
        Task AddAsync(T entity);
        void Add(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);
        void AddRange(IEnumerable<T> entities);

        Task<T?> FindAsync(Guid id);
        Task<T?> FindByMultipleKeysAsync(params object[] multipleKeys);

        /// <summary>
        /// Throws excpetion if record is not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetAsync(Guid id);

        /// <summary>
        /// Throws excpetion if record is not found
        /// </summary>
        /// <param name="multipleKeys"></param>
        /// <returns></returns>
        Task<T> GetByMultipleKeysAsync(params object[] multipleKeys);

        void Delete(T entity);

        IQueryable<T> QueryableReadonly();
        IQueryable<T> QueryableSet();
    }
}
