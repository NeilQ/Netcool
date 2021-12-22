using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
        public virtual DbContextBase ContextBase { get; private set; }

        /// <summary>
        /// Gets DbSet for given entity.
        /// </summary>
        public virtual DbSet<TEntity> Table => ContextBase.Set<TEntity>();

        public virtual DbConnection Connection
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

        public override IQueryable<TEntity> GetAll()
        {
            return GetAllIncluding();
        }

        public override IQueryable<TEntity> GetAllIncluding(
            params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = Table.AsQueryable();

            if (propertySelectors.IsNullOrEmpty()) return query;

            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }

            return query;
        }

        public override async Task<List<TEntity>> GetAllListAsync()
        {
            return await GetAll().ToListAsync();
        }

        public override async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).ToListAsync();
        }

        public override TEntity Insert(TEntity entity)
        {
            return Table.Add(entity).Entity;
        }

        public override void Insert(IEnumerable<TEntity> entities)
        {
            Table.AddRange(entities);
        }

        public override Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Insert(entity));
        }

        public override TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            ContextBase.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public override void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
        }

        public override void Delete(TPrimaryKey id)
        {
            var entity = GetFromChangeTrackerOrNull(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }

            entity = Get(id);
            if (entity == null) return;
            Delete(entity);
        }

        public override void Delete(IList<TEntity> list)
        {
            if (list == null || list.Count == 0)
                return;

            foreach (var entity in list)
            {
                AttachIfNot(entity);
            }

            Table.RemoveRange(list);
        }

        public override void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            Delete(GetAll().Where(predicate).ToList());
        }

        protected virtual void AttachIfNot(TEntity entity)
        {
            var entry = ContextBase.ChangeTracker.Entries().FirstOrDefault(ent => ent.Entity == entity);
            if (entry != null)
            {
                return;
            }

            Table.Attach(entity);
        }

        private TEntity GetFromChangeTrackerOrNull(TPrimaryKey id)
        {
            var entry = ContextBase.ChangeTracker.Entries()
                .FirstOrDefault(
                    ent =>
                        ent.Entity is TEntity entity &&
                        EqualityComparer<TPrimaryKey>.Default.Equals(id, entity.Id)
                );

            return entry?.Entity as TEntity;
        }
    }
}
