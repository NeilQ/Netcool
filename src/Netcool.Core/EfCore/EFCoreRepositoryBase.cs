using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Netcool.Core.Entities;
using Netcool.Core.Repositories;

namespace Netcool.Core.EfCore
{
    public class EfCoreRepositoryBase<TEntity> : EfCoreRepositoryBase<TEntity, int>, IRepository<TEntity>
        where TEntity : class, IEntity<int>
    {
        public EfCoreRepositoryBase(DbContextBase dbContextBase)
            : base(dbContextBase)
        {
        }
    }


    public class EfCoreRepositoryBase<TEntity, TPrimaryKey> : RepositoryBase<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        /// <summary>
        /// Gets EF DbContext object.
        /// </summary>
        public DbContextBase ContextBase { get; private set; }

        /// <summary>
        /// Gets DbSet for given entity.
        /// </summary>
        public DbSet<TEntity> Table => ContextBase.Set<TEntity>();

        public Func<IQueryable<TEntity>, IQueryable<TEntity>>? DefaultWithDetailsFunc { get; set; }

        public DbConnection Connection
        {
            get
            {
                var connection = ContextBase.Database.GetDbConnection();

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                return connection;
            }
        }

        public EfCoreRepositoryBase(DbContextBase dbContextBase)
        {
            ContextBase = dbContextBase;
        }

        public override async Task<List<TEntity>> GetListAsync()
        {
            return await GetQueryable().ToListAsync();
        }

        public override async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetQueryable().Where(predicate).ToListAsync();
        }

        public override async Task<TEntity> GetAsync(TPrimaryKey id, bool includeDetails = true)
        {
            var entity = await FindAsync(id, includeDetails);

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public override IQueryable<TEntity> GetQueryable()
        {
            return Table.AsQueryable();
        }

        public override IQueryable<TEntity> WithDetails()
        {
            if (DefaultWithDetailsFunc == null)
            {
                return GetQueryable();
            }

            return DefaultWithDetailsFunc(GetQueryable());
        }

        public override IQueryable<TEntity> WithDetails(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return IncludeDetails(
                GetQueryable(),
                propertySelectors
            );
        }

        public override async Task<TEntity> FindAsync(TPrimaryKey id, bool includeDetails = true)
        {
            return includeDetails
                ? await WithDetails().OrderBy(e => e.Id).FirstOrDefaultAsync(e => e.Id!.Equals(id))
                : await Table.FindAsync(id!);
        }

        public override async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate,
            bool includeDetails = true)
        {
            return includeDetails
                ? await WithDetails()
                    .Where(predicate)
                    .SingleOrDefaultAsync()
                : await GetQueryable()
                    .Where(predicate)
                    .SingleOrDefaultAsync();
        }

        public override async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false)
        {
            var savedEntity = Table.Add(entity).Entity;
            if (autoSave)
            {
                await ContextBase.SaveChangesAsync();
            }

            return savedEntity;
        }

        public override async Task InsertAsync(IEnumerable<TEntity> entities, bool autoSave = false)
        {
            if (!entities.Any()) return;
            Table.AddRange(entities);
            if (autoSave)
            {
                await ContextBase.SaveChangesAsync();
            }
        }

        public override async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false)
        {
            ContextBase.Attach(entity);
            var updatedEntity = ContextBase.Update(entity).Entity;
            if (autoSave)
            {
                await ContextBase.SaveChangesAsync();
            }

            return updatedEntity;
        }

        public override async Task DeleteAsync(TEntity entity, bool autoSave = false)
        {
            Table.Remove(entity);
            if (autoSave)
            {
                await ContextBase.SaveChangesAsync();
            }
        }

        public override async Task DeleteAsync(TPrimaryKey id, bool autoSave = false)
        {
            var entity = await Table.FindAsync(id);
            if (entity == null) return;
            await DeleteAsync(entity, autoSave);
        }

        public override async Task DeleteAsync(IList<TEntity> list, bool autoSave = false)
        {
            if (list == null || list.Count == 0) return;
            Table.RemoveRange(list);

            if (autoSave)
            {
                await ContextBase.SaveChangesAsync();
            }
        }

        public override async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false)
        {
            var entities = await Table
                .Where(predicate)
                .ToListAsync();
            await DeleteAsync(entities, autoSave);
        }

        public override async Task DeleteDirectAsync([NotNull] Expression<Func<TEntity, bool>> predicate)
        {
            await Table.Where(predicate).ExecuteDeleteAsync();
        }

        private static IQueryable<TEntity> IncludeDetails(
            IQueryable<TEntity> query,
            Expression<Func<TEntity, object>>[] propertySelectors)
        {
            if (propertySelectors.IsNullOrEmpty()) return query;
            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }

            return query;
        }
    }
}
