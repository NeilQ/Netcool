using Netcool.Core.Repositories;
using Netcool.Core.Services;
using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Roles
{
    public class RoleService : CrudService<Role, RoleDto, int, PageRequest, RoleSaveInput>, IRoleService
    {
        public RoleService(IRepository<Role> repository, IServiceAggregator serviceAggregator) : base(repository,
            serviceAggregator)
        {
        }
    }
}