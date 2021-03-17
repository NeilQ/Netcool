using Netcool.Api.Domain.Roles;
using Netcool.Core.Services;
using Netcool.Core.Services.Dto;

namespace Netcool.Core.Organizations
{
    public interface IOrganizationService : ICrudService<OrganizationDto, int, PageRequest, OrganizationSaveInput>
    {
    }
}