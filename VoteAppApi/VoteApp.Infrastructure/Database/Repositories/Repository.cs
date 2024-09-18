using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoteApp.Domain;

namespace VoteApp.Infrastructure.Database.Repositories
{
    public class Repository<T> : IBaseRepository<T>
            where T : class
    {
        protected VoteAppDbContext _dbContext;

        public Repository(VoteAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Add methods

        public virtual async Task AddAsync(T entity)
        {
            await _dbContext.AddAsync(entity);
        }

        public virtual void Add(T entity)
        {
            _dbContext.Add<T>(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().AddRange(entities);
        }

        #endregion

        #region Find methods

        public async Task<T?> FindAsync(Guid id)
        {
            var entity = await _dbContext.FindAsync<T>(id);

            return entity;
        }

        public async Task<T?> FindByMultipleKeysAsync(params object[] multipleKeys)
        {
            return await _dbContext.FindAsync<T>(multipleKeys);
        }

        #endregion

        #region Get methods

        public async Task<T> GetAsync(Guid id)
        {
            return await _dbContext.FindAsync<T>(new object[] { id })
                ?? throw new UnexpectedStateException($"Entity {(typeof(T)).Name} does not exist: {id}");
        }

        public async Task<T> GetByMultipleKeysAsync(params object[] multipleKeys)
        {
            return await _dbContext.FindAsync<T>(multipleKeys)
                ?? throw new UnexpectedStateException($"Entity {(typeof(T)).Name} does not exist : {multipleKeys.SerializeToJsonForLog()}");
        }

        #endregion

        #region Queryable methods

        public virtual IQueryable<T> QueryableSet()
        {
            return _dbContext.Set<T>().AsQueryable();
        }

        public virtual IQueryable<T> QueryableReadonly()
        {
            return _dbContext.Set<T>().AsNoTracking();
        }

        #endregion

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }
    }
}
