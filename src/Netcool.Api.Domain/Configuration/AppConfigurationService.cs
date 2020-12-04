using System;
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
    }
}