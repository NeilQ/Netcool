using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
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
        CrudServiceBase<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput,
            TUpdateInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        protected readonly IRepository<TEntity, TPrimaryKey> Repository;
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly ICurrentUser CurrentUser;
        protected readonly IMapper Mapper;
        protected readonly IAuthorizationService AuthorizationService;

        protected virtual string GetPermissionName { get; set; }

        protected virtual string CreatePermissionName { get; set; }

        protected virtual string UpdatePermissionName { get; set; }

        protected virtual string DeletePermissionName { get; set; }

        protected CrudServiceBase(IRepository<TEntity, TPrimaryKey> repository, IServiceAggregator serviceAggregator)
        {
            Repository = repository;
            AuthorizationService = serviceAggregator.AuthorizationService;
            UnitOfWork = serviceAggregator.UnitOfWork;
            CurrentUser = serviceAggregator.Session;
            Mapper = serviceAggregator.Mapper;
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
                query = query.Skip((request.Page.Value - 1) * request.Size.Value).Take(request.Size.Value);
            }

            return query;
        }

        protected virtual IQueryable<TEntity> ApplySort(IQueryable<TEntity> query, TGetAllInput input)
        {
            if (!(input is IPageRequest request)) return query;
            if (!string.IsNullOrEmpty(request.Sort))
            {
                query = query.OrderBy(request.Sort);
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
        /// </summary>
        /// <param name="input">The input.</param>
        protected virtual IQueryable<TEntity> CreateFilteredQuery(TGetAllInput input)
        {
            // https://github.com/dotnet/efcore/issues/14366
            return Repository.GetAll();
        }

        /// <summary>
        /// Maps <see cref="TEntity"/> to <see cref="TEntityDto"/>.
        /// </summary>
        protected virtual TEntityDto MapToEntityDto(TEntity entity)
        {
            if (entity == null) return default;
            return Mapper.Map<TEntityDto>(entity);
        }

        protected virtual TDestination MapToEntityDto<TSource, TDestination>(TSource entity)
        {
            if (entity == null) return default;
            return Mapper.Map<TDestination>(entity);
        }

        /// <summary>
        /// Maps <see cref="TEntityDto"/> to <see cref="TEntity"/> to create a new entity.
        /// </summary>
        protected virtual TEntity MapToEntity(TCreateInput createInput)
        {
            if (createInput == null) return default;
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
            if (string.IsNullOrWhiteSpace(permissionName)) return;
            var result = AuthorizationService.AuthorizeAsync(CurrentUser.ClaimsPrincipal, permissionName).Result;
            if (result.Succeeded) return;
            throw new UnauthorizedAccessException($"Grant permission[{permissionName}] failed.");
        }

        protected virtual void CheckGetPermission()
        {
            CheckPermission(GetPermissionName);
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
