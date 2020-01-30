using Netcool.Api.Domain.EfCore;
using Netcool.Core.EfCore;
using Netcool.Core.Entities;

namespace Netcool.Api.Domain.Repositories
{
    public class CommonRepository<TEntity> : EfCoreRepositoryBase<TEntity>
        where TEntity : class, IEntity<int>
    {
        public CommonRepository(NetcoolDbContext dbContext) : base(dbContext)
        {
        }
    }

    public class CommonRepository<TEntity, TPrimary> : EfCoreRepositoryBase<TEntity, TPrimary>
        where TEntity : class, IEntity<TPrimary>
    {
        public CommonRepository(NetcoolDbContext dbContext) : base(dbContext)
        {
        }
    }
}