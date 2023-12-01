using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Netcool.Core.Services;
using Netcool.Core.Services.Dto;

namespace Netcool.Core.AspNetCore.Controllers
{
    public abstract class QueryControllerBase<TEntityDto> : QueryControllerBase<TEntityDto, int, IPageRequest>
        where TEntityDto : IEntityDto<int>
    {
        protected QueryControllerBase(ICrudService<TEntityDto, int, IPageRequest, TEntityDto, TEntityDto> service) :
            base(service)
        {
        }
    }

    public abstract class
        QueryControllerBase<TEntityDto, TPrimaryKey> : QueryControllerBase<TEntityDto, TPrimaryKey, IPageRequest>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected QueryControllerBase(
            ICrudService<TEntityDto, TPrimaryKey, IPageRequest, TEntityDto, TEntityDto> service) : base(service)
        {
        }
    }

    public abstract class QueryControllerBase<TEntityDto, TPrimaryKey, TGetAllInput>
        : QueryControllerBase<TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected QueryControllerBase(
            ICrudService<TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto> service) : base(service)
        {
        }
    }

    public abstract class QueryControllerBase<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput>
        : QueryControllerBase<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TCreateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
    {
        protected QueryControllerBase(
            ICrudService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TCreateInput> service) : base(service)
        {
        }
    }

    [ApiController]
    [Produces("application/json")]
    public abstract class
        QueryControllerBase<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput> : ControllerBase
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        protected readonly ICrudService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput> Service;

        protected QueryControllerBase(
            ICrudService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput> service)
        {
            Service = service;
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TEntityDto>> Get(TPrimaryKey id)
        {
            var dto = await Service.GetAsync(id);
            return dto;
        }

        [HttpGet]
        public virtual ActionResult<IPagedResult<TEntityDto>> GetByPage([FromQuery] TGetAllInput input)
        {
            var dto = Service.GetAll(input);
            return dto;
        }

        [HttpGet("items")]
        public virtual ActionResult<IList<TEntityDto>> GetAllItems([FromQuery] TGetAllInput input)
        {
            var dto = Service.GetAll(input);
            return dto.Items.ToList();
        }
    }
}
