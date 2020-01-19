﻿using System.Collections.Generic;
using Netcool.Core.Services.Dto;

namespace Netcool.Core.Services
{
    public interface ICrudAppService<TEntityDto>
     : ICrudAppService<TEntityDto, int>
     where TEntityDto : IEntityDto<int>
    {

    }

    public interface ICrudAppService<TEntityDto, TPrimaryKey>
        : ICrudAppService<TEntityDto, TPrimaryKey, IPageRequest>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {

    }

    public interface ICrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput>
        : ICrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {

    }

    public interface ICrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput>
        : ICrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TCreateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
    {

    }

    public interface ICrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput>
        : ICrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, EntityDto<TPrimaryKey>>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {

    }

    public interface ICrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput, in TGetInput>
    : ICrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, EntityDto<TPrimaryKey>>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
    {

    }

    public interface ICrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput, in TGetInput, in TDeleteInput>
        : IService
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
        where TDeleteInput : IEntityDto<TPrimaryKey>
    {
        TEntityDto Get(TGetInput input);
        
        PagedResultDto<TEntityDto> GetAll(TGetAllInput input);

        TEntityDto Create(TCreateInput input);

        TEntityDto Update(TUpdateInput input);

        void Delete(TDeleteInput input);

        void Delete(IEnumerable<TPrimaryKey> ids);
    }
}