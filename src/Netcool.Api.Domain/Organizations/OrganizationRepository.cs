using System.Linq;
using Netcool.Api.Domain.EfCore;
using Netcool.Api.Domain.Organizations;
using Netcool.Api.Domain.Repositories;
using Netcool.Core.Repositories;

namespace Netcool.Core.Organizations
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
    }

    public class OrganizationRepository : CommonRepository<Organization, int>, IOrganizationRepository
    {
        public OrganizationRepository(NetcoolDbContext dbContext) : base(dbContext)
        {
        }

        public override IQueryable<Organization> GetAll()
        {
            return GetAllIncluding(t => t.Parent);
        }
    }
}