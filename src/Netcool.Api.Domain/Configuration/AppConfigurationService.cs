using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Netcool.Core.Repositories;
using Netcool.Core.Services;
using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Configuration
{
    public class AppConfigurationService :
        CrudService<AppConfiguration, AppConfigurationDto, int, PageRequest, AppConfigurationSaveInput>,
        IAppConfigurationService
    {
        public AppConfigurationService(IRepository<AppConfiguration> repository, IServiceAggregator serviceAggregator) :
            base(repository, serviceAggregator)
        {
        }

        public override void BeforeCreate(AppConfiguration entity)
        {
            if (Repository.FirstOrDefault(t => t.Name == entity.Name) != null) throw new ApplicationException("配置名称重复");
            base.BeforeCreate(entity);
        }

        public override void BeforeUpdate(AppConfigurationSaveInput input, AppConfiguration originEntity)
        {
            if (Repository.FirstOrDefault(t => t.Name == input.Name && t.Id != originEntity.Id) != null)
                throw new ApplicationException("配置名称重复");
            base.BeforeUpdate(input, originEntity);
        }

        protected override void BeforeDelete(IEnumerable<int> ids)
        {
            if (ids != null && ids.Any())
            {
                var configs = Repository.GetAll().AsNoTracking().Where(t => ids.Contains(t.Id)).ToList();
                if (configs.Any(t => t.IsInitial))
                {
                   throw new ApplicationException("系统初始化配置无法删除");
                }
            }

            base.BeforeDelete(ids);
        }
    }
}