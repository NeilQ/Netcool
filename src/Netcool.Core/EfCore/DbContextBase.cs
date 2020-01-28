﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Netcool.Core.Entities;
using Netcool.Core.Helpers;
using Netcool.Core.Sessions;

namespace Netcool.Core.EfCore
{
    public class DbContextBase : DbContext
    {

        public DbContextBase(DbContextOptions options) : base(options)
        {
        }

        private static readonly MethodInfo ConfigureGlobalFiltersMethodInfo =
            typeof(DbContextBase).GetMethod(nameof(ConfigureGlobalFilters),
                BindingFlags.Instance | BindingFlags.NonPublic);

        public IUserSession UserSession { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                ConfigureGlobalFiltersMethodInfo
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] {modelBuilder, entityType});
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
                Expression<Func<TEntity, bool>> softDeleteFilter = e => !((ISoftDelete) e).IsDeleted;
                expression = softDeleteFilter;
            }

            if (typeof(IHasTenant).IsAssignableFrom(typeof(TEntity)))
            {
                Expression<Func<TEntity, bool>> mustHaveTenantFilter =
                    e => ((IHasTenant) e).TenantId == UserSession.TenantId;
                expression = expression == null
                    ? mustHaveTenantFilter
                    : CombineExpressions(expression, mustHaveTenantFilter);
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
            var userId = UserSession?.UserId ?? 0;
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
                    ApplyModifiedEntity(entry, userId);
                    break;
                case EntityState.Deleted:
                    ApplyDeletedEntity(entry, userId);
                    break;
            }
        }

        protected virtual void ApplyAddedEntity(EntityEntry entry, int userId)
        {
            CheckAndSetId(entry);
            SetCreateAuditProperties(entry, userId);
            SetTenantProperties(entry, UserSession.TenantId);
        }

        protected virtual void ApplyModifiedEntity(EntityEntry entry, int userId)
        {
            switch (entry.Entity)
            {
                case ISoftDelete sdAudit when sdAudit.IsDeleted:
                    SetSoftDeleteProperties(entry, userId);
                    break;
                case IUpdateAudit _:
                    SetUpdateAuditProperties(entry, userId);
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
            if (entry.Entity is IEntity<Guid> entity && entity.Id == Guid.Empty)
            {
                var idPropertyEntry = entry.Property("Id");

                if (idPropertyEntry != null && idPropertyEntry.Metadata.ValueGenerated == ValueGenerated.Never)
                {
                    entity.Id = Guid.NewGuid();
                }
            }
        }

        private void SetTenantProperties(EntityEntry entry, int tenantId)
        {
            if (!(entry.Entity is IHasTenant tenant)) return;
            if (tenant.TenantId > 0)
            {
                tenant.TenantId = tenantId;
            }
        }

        private void SetCreateAuditProperties(EntityEntry entry, int userId)
        {
            if (!(entry.Entity is ICreateAudit audit)) return;
            if (audit.CreateUserId == null || audit.CreateUserId <= 0)
            {
                audit.CreateUserId = userId;
            }

            if (audit.CreateTime != null)
            {
                audit.CreateTime = DateTime.Now;
            }
        }

        private void SetUpdateAuditProperties(EntityEntry entry, int userId)
        {
            if (!(entry.Entity is IUpdateAudit audit)) return;
            if (audit.UpdateUserId == null || audit.UpdateUserId <= 0)
            {
                audit.UpdateUserId = userId;
            }

            if (audit.UpdateTime == null)
            {
                audit.UpdateTime = DateTime.Now;
            }
        }

        private void SetSoftDeleteProperties(EntityEntry entry, int userId)
        {
            if (!(entry.Entity is IDeleteAudit dAudit)) return;
            if (dAudit.DeleteUserId == null || dAudit.DeleteUserId <= 0)
            {
                dAudit.DeleteUserId = userId;
            }

            if (dAudit.DeleteTime == null)
            {
                dAudit.DeleteTime = DateTime.Now;
            }
        }

        private Expression<Func<T, bool>> CombineExpressions<T>(Expression<Func<T, bool>> expression1,
            Expression<Func<T, bool>> expression2)
        {
            return ExpressionCombiner.Combine(expression1, expression2);
        }
    }
}