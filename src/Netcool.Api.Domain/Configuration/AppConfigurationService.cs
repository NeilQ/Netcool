using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Netcool.Core.Repositories;
using Netcool.Core.Services;

namespace Netcool.Api.Domain.Configuration
{
    public sealed class AppConfigurationService :
        CrudService<AppConfiguration, AppConfigurationDto, int, AppConfigurationRequest, AppConfigurationSaveInput>,
        IAppConfigurationService
    {
        public AppConfigurationService(IRepository<AppConfiguration> repository, IServiceAggregator serviceAggregator) :
            base(repository, serviceAggregator)
        {
            GetPermissionName = "config.view";
            CreatePermissionName = "config.create";
            UpdatePermissionName = "config.update";
            DeletePermissionName = "config.delete";
        }

        protected override IQueryable<AppConfiguration> CreateFilteredQuery(AppConfigurationRequest input)
        {
            var query = base.CreateFilteredQuery(input);
            if (!string.IsNullOrEmpty(input.Name))
            {
                query = query.Where(t => t.Name == input.Name);
            }

            return query;
        }

        public override async Task BeforeCreate(AppConfiguration entity)
        {
            await base.BeforeCreate(entity);
            if (Repository.GetAll().FirstOrDefault(t => t.Name == entity.Name) != null)
                throw new ApplicationException("配置名称重复");
        }

        public override async Task BeforeUpdate(AppConfigurationSaveInput input, AppConfiguration originEntity)
        {
            await base.BeforeUpdate(input, originEntity);
            if (Repository.GetAll().FirstOrDefault(t => t.Name == input.Name && t.Id != originEntity.Id) != null)
                throw new ApplicationException("配置名称重复");
        }

        protected override void BeforeDelete(IEnumerable<int> ids)
        {
            base.BeforeDelete(ids);
            if (ids != null && ids.Any())
            {
                var configs = Repository.GetAll().AsNoTracking().Where(t => ids.Contains(t.Id)).ToList();
                if (configs.Any(t => t.IsInitial))
                {
                    throw new ApplicationException("系统初始化配置无法删除");
                }
            }
        }
    }
}
