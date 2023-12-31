using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        public abstract IQueryable<TEntity> GetQueryable();

        public abstract Task<List<TEntity>> GetListAsync();

        public abstract Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);

        public virtual IQueryable<TEntity> WithDetails()
        {
            return GetQueryable();
        }

        public virtual IQueryable<TEntity> WithDetails(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return GetQueryable();
        }

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

        public abstract Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false);

        public abstract Task InsertAsync(IEnumerable<TEntity> entities, bool autoSave = false);

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

        /// <summary>
        /// Deletes all entities those fit to the given predicate.
        /// It directly deletes entities from database, without fetching them.
        /// Some features (like soft-delete, multi-tenancy and audit logging) won't work, so use this method carefully when you need it.
        /// Use the DeleteAsync method if you need to these features.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        public abstract Task DeleteDirectAsync([NotNull] Expression<Func<TEntity, bool>> predicate);

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
