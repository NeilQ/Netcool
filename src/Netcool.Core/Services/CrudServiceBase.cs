using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using AutoMapper;
using Netcool.Core.Entities;
using Netcool.Core.Repositories;
using Netcool.Core.Services.Dto;
using Netcool.Core.Sessions;

namespace Netcool.Core.Services
{
    /// <summary>
    /// This is a common base class for CrudAppService and AsyncCrudAppService classes.
    /// Inherit either from CrudAppService or AsyncCrudAppService, not from this class.
    /// </summary>
    public abstract class
        CrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput,
            TUpdateInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        protected readonly IRepository<TEntity, TPrimaryKey> Repository;
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly INetcoolSession Session;
        protected readonly IMapper Mapper;

        protected virtual string GetPermissionName { get; set; }

        protected virtual string GetAllPermissionName { get; set; }

        protected virtual string CreatePermissionName { get; set; }

        protected virtual string UpdatePermissionName { get; set; }

        protected virtual string DeletePermissionName { get; set; }

        protected CrudAppServiceBase(IRepository<TEntity, TPrimaryKey> repository, IUnitOfWork unitOfWork,
            INetcoolSession session, IMapper mapper)
        {
            Repository = repository;
            UnitOfWork = unitOfWork;
            Session = session;
            Mapper = mapper;
        }

        /// <summary>
        /// Should apply paging if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected virtual IQueryable<TEntity> ApplyPaging(IQueryable<TEntity> query, TGetAllInput input)
        {
            if (!(input is IPageRequest request)) return query;

            if (request.Page > 0 && request.Size > 0)
            {
                query = query.Skip((request.Page - 1) * request.Size).Take(request.Size);
            }

            if (!string.IsNullOrEmpty(request.Sort))
            {
                query = query.OrderBy("");
            }
            else
            {
                query = query.OrderByDescending(t => t.Id);
            }

            return query;
        }

        /// <summary>
        /// This method should create <see cref="IQueryable{TEntity}"/> based on given input.
        /// It should filter query if needed, but should not do sorting or paging.
        /// Sorting should be done in <see cref="ApplySorting"/> and paging should be done in <see cref="ApplyPaging"/>
        /// methods.
        /// </summary>
        /// <param name="input">The input.</param>
        protected virtual IQueryable<TEntity> CreateFilteredQuery(TGetAllInput input)
        {
            return Repository.GetAll();
        }

        /// <summary>
        /// Maps <see cref="TEntity"/> to <see cref="TEntityDto"/>.
        /// </summary>
        protected virtual TEntityDto MapToEntityDto(TEntity entity)
        {
            return Mapper.Map<TEntityDto>(entity);
        }

        /// <summary>
        /// Maps <see cref="TEntityDto"/> to <see cref="TEntity"/> to create a new entity.
        /// </summary>
        protected virtual TEntity MapToEntity(TCreateInput createInput)
        {
            return Mapper.Map<TEntity>(createInput);
        }

        /// <summary>
        /// Maps <see cref="TUpdateInput"/> to <see cref="TEntity"/> to update the entity.
        /// </summary>
        protected virtual void MapToEntity(TUpdateInput updateInput, TEntity entity)
        {
            Mapper.Map(updateInput, entity);
        }

        protected virtual void CheckPermission(string permissionName)
        {
            if (!string.IsNullOrEmpty(permissionName))
            {
                // PermissionChecker.Authorize(permissionName);
                // TODO: permission checker implement
            }
        }

        protected virtual void CheckGetPermission()
        {
            CheckPermission(GetPermissionName);
        }

        protected virtual void CheckGetAllPermission()
        {
            CheckPermission(GetAllPermissionName);
        }

        protected virtual void CheckCreatePermission()
        {
            CheckPermission(CreatePermissionName);
        }

        protected virtual void CheckUpdatePermission()
        {
            CheckPermission(UpdatePermissionName);
        }

        protected virtual void CheckDeletePermission()
        {
            CheckPermission(DeletePermissionName);
        }
    }
}