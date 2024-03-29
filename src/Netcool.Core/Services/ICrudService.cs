﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Netcool.Core.Services.Dto;

namespace Netcool.Core.Services
{
    public interface ICrudService<TEntityDto>
     : ICrudService<TEntityDto, int>
     where TEntityDto : IEntityDto<int>
    {

    }

    public interface ICrudService<TEntityDto, TPrimaryKey>
        : ICrudService<TEntityDto, TPrimaryKey, IPageRequest>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {

    }

    public interface ICrudService<TEntityDto, TPrimaryKey, in TGetAllInput>
        : ICrudService<TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {

    }

    public interface ICrudService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput>
        : ICrudService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TCreateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
    {

    }

    public interface ICrudService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput>
        : ICrudService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, EntityDto<TPrimaryKey>>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {

    }

    public interface ICrudService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput, in TGetInput>
    : ICrudService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, EntityDto<TPrimaryKey>>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
    {

    }

    public interface ICrudService<TEntityDto, in TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput, in TGetInput, in TDeleteInput>
        : IService
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
        where TDeleteInput : IEntityDto<TPrimaryKey>
    {
        Task<TEntityDto> GetAsync(TPrimaryKey id);
        
        PagedResultDto<TEntityDto> GetAll(TGetAllInput input);

        Task<TEntityDto> CreateAsync(TCreateInput input);

        Task<TEntityDto> UpdateAsync(TUpdateInput input);

        Task DeleteAsync(TPrimaryKey id);

        Task DeleteAsync(IEnumerable<TPrimaryKey> ids);
    }
}
