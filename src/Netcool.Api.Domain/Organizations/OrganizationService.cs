using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Netcool.Api.Domain.Organizations;
using Netcool.Core.Entities;
using Netcool.Core.Repositories;
using Netcool.Core.Services;
using Netcool.Core.Services.Dto;

namespace Netcool.Core.Organizations;

public sealed class OrganizationService :
    CrudService<Organization, OrganizationDto, int, PageRequest, OrganizationSaveInput>, IOrganizationService
{
    public OrganizationService(IRepository<Organization, int> repository, IServiceAggregator serviceAggregator) :
        base(repository, serviceAggregator)
    {
        GetPermissionName = "organization.view";
        UpdatePermissionName = "organization.update";
        CreatePermissionName = "organization.create";
        DeletePermissionName = "organization.delete";
    }

    public override async Task<OrganizationDto> CreateAsync(OrganizationSaveInput input)
    {
        var entity = MapToEntity(input);
        await BeforeCreate(entity);

        Organization parent = null;
        if (entity.ParentId > 0)
        {
            parent = await GetEntityByIdAsync(entity.ParentId.Value);
            if (parent == null) throw new EntityNotFoundException(typeof(Organization), entity.ParentId);
            entity.Depth = parent.Depth + 1;
        }
        else
        {
            entity.ParentId = null;
            entity.Depth = 1;
        }

        using (var scope = UnitOfWork.BeginTransactionScope())
        {
            await Repository.InsertAsync(entity);
            await UnitOfWork.SaveChangesAsync();

            entity.Path = parent == null ? $"/{entity.Id}" : $"{parent.Path}/{entity.Id}";
            await UnitOfWork.SaveChangesAsync();
            scope.Complete();
        }

        return MapToEntityDto(entity);
    }

    public override async Task BeforeUpdate(OrganizationSaveInput input, Organization originEntity)
    {
        await base.BeforeUpdate(input, originEntity);
        input.ParentId = originEntity.ParentId;
    }

    public override async Task DeleteAsync(int id)
    {
        await DeleteAsync(new[] { id });
    }

    public override async Task DeleteAsync(IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any()) return;

        var entities = Repository.GetQueryable().Where(t => ids.Contains(t.Id)).ToList();
        for (var iEntity = 0; iEntity < entities.Count; iEntity++)
        {
            var entity = entities[iEntity];
            if (entity == null) continue;
            var children = Repository.GetQueryable().Where(t => t.Path.StartsWith(entity.Path.TrimEnd('/') + "/"))
                .ToList();

            if (children.Count > 0)
            {
                entities.AddRange(children);
            }
        }

        await Repository.DeleteAsync(entities);
        await UnitOfWork.SaveChangesAsync();
    }
}
