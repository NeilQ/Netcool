using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Netcool.Core.Entities;

namespace Netcool.Core.Repositories
{
    public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : class, IEntity<int>
    {
    }

    /// <summary>
    /// This interface is implemented by all repositories to ensure implementation of fixed methods.
    /// </summary>
    /// <typeparam name="TEntity">Main Entity type this repository works on</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key type of the entity</typeparam>
    public interface IRepository<TEntity, TPrimaryKey> : IRepository where TEntity : class, IEntity<TPrimaryKey>
    {
        #region Select/Get/Query

        /// <summary>
        /// Used to get a IQueryable that is used to retrieve entities from entire table.
        /// </summary>
        /// <returns>IQueryable to be used to select entities from database</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Used to get a IQueryable that is used to retrieve entities from entire table.
        /// One or more 
        /// </summary>
        /// <param name="propertySelectors">A list of include expressions.</param>
        /// <returns>IQueryable to be used to select entities from database</returns>
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors);

        /// <summary>
        /// Used to get all entities.
        /// </summary>
        /// <returns>List of all entities</returns>
        List<TEntity> GetAllList();

        /// <summary>
        /// Used to get all entities.
        /// </summary>
        /// <returns>List of all entities</returns>
        Task<List<TEntity>> GetAllListAsync();

        /// <summary>
        /// Used to get all entities based on given <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <returns>List of all entities</returns>
        List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Used to get all entities based on given <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <returns>List of all entities</returns>
        Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Used to run a query over entire entities.
        /// if <paramref name="queryMethod"/> finishes IQueryable with ToList, FirstOrDefault etc..
        /// </summary>
        /// <typeparam name="T">Type of return value of this method</typeparam>
        /// <param name="queryMethod">This method is used to query over entities</param>
        /// <returns>Query result</returns>
        T Query<T>(Func<IQueryable<TEntity>, T> queryMethod);

        /// <summary>
        /// Gets an entity with given primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity to get</param>
        /// <returns>Entity</returns>
        TEntity Get(TPrimaryKey id);

        /// <summary>
        /// Gets an entity with given primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity to get</param>
        /// <returns>Entity</returns>
        Task<TEntity> GetAsync(TPrimaryKey id);

        #endregion

        #region Insert

        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <param name="entity">Inserted entity</param>
        TEntity Insert(TEntity entity);

        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <param name="entity">Inserted entity</param>
        Task<TEntity> InsertAsync(TEntity entity);

        void Insert(IEnumerable<TEntity> entities);

        Task InsertAsync(IEnumerable<TEntity> entities);

        #endregion

        #region Update

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        TEntity Update(TEntity entity);

        /// <summary>
        /// Updates an existing entity. 
        /// </summary>
        /// <param name="entity">Entity</param>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        /// <param name="updateAction">Action that can be used to change values of the entity</param>
        /// <returns>Updated entity</returns>
        TEntity Update(TPrimaryKey id, Action<TEntity> updateAction);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        /// <param name="updateAction">Action that can be used to change values of the entity</param>
        /// <returns>Updated entity</returns>
        Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction);

        #endregion

        #region Delete

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">Entity to be deleted</param>
        /// <param name="autoSave">
        /// Set true to automatically save changes to database.
        /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.
        /// </param>
        Task DeleteAsync([NotNull] TEntity entity, bool autoSave = false);

        /// <summary>
        /// Deletes an entity by primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity</param>
        ///    <param name="autoSave">
        /// Set true to automatically save changes to database.
        /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.
        /// </param>
        Task DeleteAsync(TPrimaryKey id, bool autoSave = false);

        Task DeleteAsync([NotNull] IList<TEntity> list, bool autoSave = false);

        /// <summary>
        ///  Deletes many entities by the given <paramref name="predicate"/>.
        /// Notice that: All entities fits to given predicate are retrieved and deleted.
        /// This may cause major performance problems if there are too many entities with
        /// given predicate.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="autoSave">
        /// Set true to automatically save changes to database.
        /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.
        /// </param>
        Task DeleteAsync([NotNull] Expression<Func<TEntity, bool>> predicate, bool autoSave = false);

        #endregion
    }

    public interface IRepository
    {
    }
}
