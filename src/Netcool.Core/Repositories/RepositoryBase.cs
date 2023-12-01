using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Netcool.Core.Entities;

namespace Netcool.Core.Repositories
{
    public abstract class RepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        #region Query

        public abstract IQueryable<TEntity> GetAll();

        public virtual IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return GetAll();
        }

        public virtual List<TEntity> GetAllList()
        {
            return GetAll().ToList();
        }

        public virtual Task<List<TEntity>> GetAllListAsync()
        {
            return Task.FromResult(GetAllList());
        }

        public virtual List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ToList();
        }

        public virtual Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(GetAllList(predicate));
        }

        public virtual T Query<T>(Func<IQueryable<TEntity>, T> queryMethod)
        {
            return queryMethod(GetAll());
        }

        public abstract IQueryable<TEntity> WithDetails(params Expression<Func<TEntity, object>>[] propertySelectors);
        
        public abstract IQueryable<TEntity> GetQueryable();
        
        public abstract Task<TEntity> GetAsync(TPrimaryKey id, bool includeDetails = true);

        public abstract Task<TEntity> FindAsync(TPrimaryKey id, bool includeDetails = true);

        public abstract Task<TEntity> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool includeDetails = true);

        public async Task<TEntity> GetAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool includeDetails = true)
        {
            var entity = await FindAsync(predicate, includeDetails);

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity));
            }

            return entity;
        }


        #endregion

        #region insert

        public abstract TEntity Insert(TEntity entity);

        public virtual Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Insert(entity));
        }

        public abstract void Insert(IEnumerable<TEntity> entities);

        public virtual Task InsertAsync(IEnumerable<TEntity> entities)
        {
            Insert(entities);
            return Task.FromResult(0);
        }
        #endregion

        #region update

        public abstract Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false);

        public virtual async Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction)
        {
            var entity = await GetAsync(id);
            await updateAction(entity);
            return entity;
        }

        #endregion

        #region delete

        public abstract Task DeleteAsync(TEntity entity, bool autoSave = false);

        public abstract Task DeleteAsync(TPrimaryKey id, bool autoSave = false);

        public abstract Task DeleteAsync(IList<TEntity> list, bool autoSave = false);

        public abstract Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false);

        #endregion

        protected virtual Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TPrimaryKey))
            );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
    }
}
