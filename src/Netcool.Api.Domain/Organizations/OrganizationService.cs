using System.Collections.Generic;
using System.Linq;
using Netcool.Api.Domain.Organizations;
using Netcool.Core.Entities;
using Netcool.Core.Repositories;
using Netcool.Core.Services;
using Netcool.Core.Services.Dto;

namespace Netcool.Core.Organizations
{
    public class OrganizationService :
        CrudService<Organization, OrganizationDto, int, PageRequest, OrganizationSaveInput>, IOrganizationService
    {
        public OrganizationService(IRepository<Organization, int> repository, IServiceAggregator serviceAggregator) :
            base(repository, serviceAggregator)
        {
        }

        public override OrganizationDto Create(OrganizationSaveInput input)
        {
            var entity = MapToEntity(input);
            BeforeCreate(entity);

            Organization parent = null;
            if (entity.ParentId > 0)
            {
                parent = GetEntityById(entity.ParentId.Value);
                if (parent == null) throw new EntityNotFoundException(typeof(Organization), entity.ParentId);
                entity.Depth = parent.Depth + 1;
            }
            else
            {
                entity.ParentId = null;
                entity.Depth = 1;
            }

            using var scope = UnitOfWork.BeginTransactionScope();
            Repository.Insert(entity);
            UnitOfWork.SaveChanges();

            entity.Path = parent == null ? $"/{entity.Id}" : $"{parent.Path}/{entity.Id}";
            Repository.Update(entity);
            UnitOfWork.SaveChanges();
            scope.Complete();

            return MapToEntityDto(entity);
        }

        public override void BeforeUpdate(OrganizationSaveInput input, Organization originEntity)
        {
            input.ParentId = originEntity.ParentId;
        }

        public override void Delete(int id)
        {
            Delete(new[] {id});
        }

        public override void Delete(IEnumerable<int> ids)
        {
            if (ids == null || !ids.Any()) return;

            var entities = Repository.GetAll().Where(t => ids.Contains(t.Id)).ToList();
            for (var iEntity = 0; iEntity < entities.Count; iEntity++)
            {
                var entity = entities[iEntity];
                if (entity == null) continue;
                var children = Repository.GetAll().Where(t => t.Path.StartsWith(entity.Path.TrimEnd('/') + "/"))
                    .ToList();

                if (children.Count > 0)
                {
                    entities.AddRange(children);
                }
            }

            Repository.Delete(entities);
            UnitOfWork.SaveChanges();
        }
    }
}
