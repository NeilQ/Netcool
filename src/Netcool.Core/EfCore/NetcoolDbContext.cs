using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Netcool.Core.Entities;
using Netcool.Core.Sessions;

namespace Netcool.Core.EfCore
{
    public class NetcoolDbContext : DbContext
    {
        public NetcoolDbContext(DbContextOptions<NetcoolDbContext> options) : base(options) { }

        private static readonly MethodInfo ConfigureGlobalFiltersMethodInfo = typeof(NetcoolDbContext).GetMethod(nameof(ConfigureGlobalFilters), BindingFlags.Instance | BindingFlags.NonPublic);

        public INetcoolSession NetcoolSession { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                ConfigureGlobalFiltersMethodInfo
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder, entityType });
            }
        }

        protected void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType entityType)
            where TEntity : class
        {
            if (entityType.BaseType != null) return;

            var filterExpression = CreateFilterExpression<TEntity>();
            if (filterExpression != null)
            {
                modelBuilder.Entity<TEntity>().HasQueryFilter(filterExpression);
            }
        }


        protected virtual Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>()
            where TEntity : class
        {
            Expression<Func<TEntity, bool>> expression = null;

            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                Expression<Func<TEntity, bool>> softDeleteFilter = e => !((ISoftDelete)e).IsDeleted;
                expression = softDeleteFilter;
            }

            return expression;
        }

        public override int SaveChanges()
        {
            SaveChangesBefore();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            SaveChangesBefore();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected virtual void SaveChangesBefore()
        {
            var userId = NetcoolSession?.UserId ?? 0;
            foreach (var entry in ChangeTracker.Entries().ToList())
            {
                if (entry.State != EntityState.Modified && entry.CheckOwnedEntityChange())
                {
                    Entry(entry.Entity).State = EntityState.Modified;
                    SaveChangesBefore(entry, userId);
                }
            }
        }

        protected virtual void SaveChangesBefore(EntityEntry entry, int userId)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    ApplyAddedEntity(entry, userId);
                    break;
                case EntityState.Modified:
                    ApplModifiedEntity(entry, userId);
                    break;
                case EntityState.Deleted:
                    ApplyDeletedEntity(entry, userId);
                    break;
            }
        }

        protected virtual void ApplyAddedEntity(EntityEntry entry, int userId)
        {
            CheckAndSetId(entry);

            if (!(entry.Entity is ICreateAudit audit)) return;
            audit.CreateUserId = userId;
            audit.CreateTime = DateTime.Now;
        }

        protected virtual void ApplModifiedEntity(EntityEntry entry, int userId)
        {
            switch (entry.Entity)
            {
                case ISoftDelete sdAudit when sdAudit.IsDeleted:
                    SetSoftDeleteProperties(entry, userId);
                    break;
                case IUpdateAudit audit:
                    audit.UpdateTime = DateTime.Now;
                    audit.UpdateUserId = userId;
                    break;
            }
        }

        protected virtual void ApplyDeletedEntity(EntityEntry entry, int userId)
        {
            // Check if the entity is ISoftDelete, if true, then change to soft delete operation
            if (!(entry.Entity is ISoftDelete sd)) return;
            entry.Reload();
            entry.State = EntityState.Modified;
            sd.IsDeleted = true;
            SetSoftDeleteProperties(entry, userId);
        }


        protected virtual void CheckAndSetId(EntityEntry entry)
        {
            //Set GUID Ids
            var entity = entry.Entity as IEntity<Guid>;
            if (entity != null && entity.Id == Guid.Empty)
            {
                var idPropertyEntry = entry.Property("Id");

                if (idPropertyEntry != null && idPropertyEntry.Metadata.ValueGenerated == ValueGenerated.Never)
                {
                    entity.Id = Guid.NewGuid();
                }
            }
        }

        private void SetSoftDeleteProperties(EntityEntry entry, int userId)
        {
            if (!(entry.Entity is IDeleteAudit dAudit)) return;
            dAudit.DeleteTime = DateTime.Now;
            dAudit.DeleteUserId = userId;
        }

    }
}