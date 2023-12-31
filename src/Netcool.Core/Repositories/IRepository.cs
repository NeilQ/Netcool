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
        /// Get a single entity by the given <paramref name="predicate"/>.
        /// <para>
        /// It returns null if there is no entity with the given <paramref name="predicate"/>.
        /// It throws <see cref="InvalidOperationException"/> if there are multiple entities with the given <paramref name="predicate"/>.
        /// </para>
        /// </summary>
        /// <param name="predicate">A condition to find the entity</param>
        /// <param name="includeDetails">Set true to include all children of this entity</param>
        Task<TEntity> FindAsync(
            [NotNull] Expression<Func<TEntity, bool>> predicate,
            bool includeDetails = true);

        /// <summary>
        /// Get a single entity by the given <paramref name="id"/>.
        /// <para>
        /// It returns null if there is no entity with the given <paramref name="id"/>.
        /// It throws <see cref="InvalidOperationException"/> if there are multiple entities with the given <paramref name="id"/>.
        /// </para>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeDetails">Set true to include all children of this entity</param>
        Task<TEntity> FindAsync(TPrimaryKey id, bool includeDetails = true);

        /// <summary>
        /// Get a single entity by the given <paramref name="predicate"/>.
        /// <para>
        /// It throws <see cref="EntityNotFoundException"/> if there is no entity with the given <paramref name="predicate"/>.
        /// It throws <see cref="InvalidOperationException"/> if there are multiple entities with the given <paramref name="predicate"/>.
        /// </para>
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="includeDetails">Set true to include all children of this entity</param>
        Task<TEntity> GetAsync(
            [NotNull] Expression<Func<TEntity, bool>> predicate,
            bool includeDetails = true);

        /// <summary>
        /// Get a single entity by the given <paramref name="id"/>.
        /// <para>
        /// It throws <see cref="EntityNotFoundException"/> if there is no entity with the given <paramref name="id"/>.
        /// It throws <see cref="InvalidOperationException"/> if there are multiple entities with the given <paramref name="id"/>.
        /// </para>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeDetails">Set true to include all children of this entity</param>
        Task<TEntity> GetAsync(TPrimaryKey id, bool includeDetails = true);

        #endregion

        #region Insert

        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <param name="entity">Inserted entity</param>
        /// <param name="autoSave"></param>
        Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false);

        Task InsertAsync(IEnumerable<TEntity> entities, bool autoSave = false);

        #endregion

        #region Update

        /// <summary>
        /// Updates an existing entity. 
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="autoSave"></param>
        Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false);

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

        /// <summary>
        /// Deletes all entities those fit to the given predicate.
        /// It directly deletes entities from database, without fetching them.
        /// Some features (like soft-delete, multi-tenancy and audit logging) won't work, so use this method carefully when you need it.
        /// Use the DeleteAsync method if you need to these features.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        Task DeleteDirectAsync([NotNull] Expression<Func<TEntity, bool>> predicate);

        #endregion
    }

    public interface IRepository
    {
    }
}
