﻿using System;
using System.Collections.Generic;
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
    public class DbContextBase : DbContext, IDbContext
    {
        public DbContextBase(DbContextOptions options, ICurrentUser currentUser) : base(options)
        {
            CurrentUser = currentUser;
        }

        private static readonly MethodInfo ConfigureGlobalFiltersMethodInfo =
            typeof(DbContextBase).GetMethod(nameof(ConfigureGlobalFilters),
                BindingFlags.Instance | BindingFlags.NonPublic);

        public ICurrentUser CurrentUser { get; set; }

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

            if (typeof(IHasTenant).IsAssignableFrom(typeof(TEntity)))
            {
                Expression<Func<TEntity, bool>> mustHaveTenantFilter =
                    e => EF.Property<int?>(e, nameof(IHasTenant.TenantId)) == null ||
                         !(EF.Property<int?>(e, nameof(IHasTenant.TenantId)) > 0) ||
                         EF.Property<int?>(e, nameof(IHasTenant.TenantId)) == CurrentUser.TenantId;
                expression = expression == null
                    ? mustHaveTenantFilter
                    : CombineExpressions(expression, mustHaveTenantFilter);
            }


            return expression;
        }


        public override int SaveChanges()
        {
            var events = SaveChangesBefore();
            var result = base.SaveChanges();
            SaveChangesAfter(events);

            return result;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            var events = SaveChangesBefore();
            var result = base.SaveChangesAsync(cancellationToken);
            SaveChangesAfter(events);

            return result;
        }

        protected virtual List<EntityChangeEvent> SaveChangesBefore()
        {
            var userId = CurrentUser?.UserId ?? 0;
            var events = new List<EntityChangeEvent>();
            foreach (var entry in ChangeTracker.Entries().ToList())
            {
                if (entry.State != EntityState.Modified && entry.CheckOwnedEntityChange())
                {
                    Entry(entry.Entity).State = EntityState.Modified;
                }

                SaveChangesBefore(entry, userId, events);
            }

            return events;
        }

        protected virtual void SaveChangesBefore(EntityEntry entry, int userId,
            List<EntityChangeEvent> entityChangeEvents)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    ApplyAddedEntity(entry, userId, entityChangeEvents);
                    break;
                case EntityState.Modified:
                    ApplyModifiedEntity(entry, userId, entityChangeEvents);
                    break;
                case EntityState.Deleted:
                    ApplyDeletedEntity(entry, userId, entityChangeEvents);
                    break;
            }
        }

        protected virtual void SaveChangesAfter(List<EntityChangeEvent> entityChangeEvents)
        {
            if (entityChangeEvents == null || entityChangeEvents.Count <= 0) return;
            foreach (var change in entityChangeEvents)
            {
                EntityChangeObserver.Instance.OnChanged(change);
            }
        }

        protected virtual void ApplyAddedEntity(EntityEntry entry, int userId,
            List<EntityChangeEvent> entityChangeEvents)
        {
            CheckAndSetId(entry);
            SetCreateAuditProperties(entry, userId);
            SetUpdateAuditProperties(entry, userId);
            SetTenantProperties(entry, CurrentUser.TenantId);
            entityChangeEvents.Add(new EntityChangeEvent(entry.Entity, EntityChangeType.Created));
        }

        protected virtual void ApplyModifiedEntity(EntityEntry entry, int userId,
            List<EntityChangeEvent> entityChangeEvents)
        {
            switch (entry.Entity)
            {
                case ISoftDelete { IsDeleted: true }:
                    SetSoftDeleteProperties(entry, userId);
                    entityChangeEvents.Add(new EntityChangeEvent(entry.Entity, EntityChangeType.Deleted));
                    break;
                case IUpdateAudit _:
                    SetUpdateAuditProperties(entry, userId);
                    entityChangeEvents.Add(new EntityChangeEvent(entry.Entity, EntityChangeType.Updated));
                    break;
            }
        }

        protected virtual void ApplyDeletedEntity(EntityEntry entry, int userId,
            List<EntityChangeEvent> entityChangeEvents)
        {
            // Check if the entity is ISoftDelete, if true, then change to soft delete operation
            if (entry.Entity is not ISoftDelete sd) return;
            entry.Reload();
            sd.IsDeleted = true;
            SetSoftDeleteProperties(entry, userId);
            entityChangeEvents.Add(new EntityChangeEvent(entry.Entity, EntityChangeType.Deleted));
        }

        protected virtual void CheckAndSetId(EntityEntry entry)
        {
            //Set GUID Ids
            if (entry.Entity is IEntity<Guid> entity && entity.Id == Guid.Empty)
            {
                var idPropertyEntry = entry.Property("Id");

                if (idPropertyEntry.Metadata.ValueGenerated == ValueGenerated.Never)
                {
                    entity.Id = Guid.NewGuid();
                }
            }
        }

        private void SetTenantProperties(EntityEntry entry, int? tenantId)
        {
            if (entry.Entity is not IHasTenant tenant) return;
            if (tenant.TenantId is > 0)
            {
                tenant.TenantId = tenantId;
            }
        }

        private void SetCreateAuditProperties(EntityEntry entry, int userId)
        {
            if (entry.Entity is not ICreateAudit audit) return;
            if (audit.CreateUserId is null or <= 0)
            {
                audit.CreateUserId = userId;
            }

            audit.CreateTime ??= DateTime.Now;
        }

        private void SetUpdateAuditProperties(EntityEntry entry, int userId)
        {
            if (!(entry.Entity is IUpdateAudit audit)) return;
            if (audit.UpdateUserId == null || audit.UpdateUserId <= 0)
            {
                audit.UpdateUserId = userId;
            }

            audit.UpdateTime = DateTime.Now;
        }

        private void SetSoftDeleteProperties(EntityEntry entry, int userId)
        {
            if (entry.Entity is not IDeleteAudit dAudit) return;
            if (dAudit.DeleteUserId is null or <= 0)
            {
                dAudit.DeleteUserId = userId;
            }

            dAudit.DeleteTime ??= DateTime.Now;
        }

        private Expression<Func<T, bool>> CombineExpressions<T>(Expression<Func<T, bool>> expression1,
            Expression<Func<T, bool>> expression2)
        {
            return ExpressionCombiner.Combine(expression1, expression2);
        }
    }
}
