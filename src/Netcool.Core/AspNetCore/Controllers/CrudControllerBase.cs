using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Netcool.Core.Services;
using Netcool.Core.Services.Dto;

namespace Netcool.Core.AspNetCore.Controllers
{
    public abstract class CrudControllerBase<TEntityDto> : CrudControllerBase<TEntityDto, int, IPageRequest>
        where TEntityDto : IEntityDto<int>
    {
        protected CrudControllerBase(ICrudService<TEntityDto, int, IPageRequest, TEntityDto, TEntityDto> service) :
            base(service)
        {
        }
    }

    public abstract class
        CrudControllerBase<TEntityDto, TPrimaryKey> : CrudControllerBase<TEntityDto, TPrimaryKey, IPageRequest>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected CrudControllerBase(
            ICrudService<TEntityDto, TPrimaryKey, IPageRequest, TEntityDto, TEntityDto> service) : base(service)
        {
        }
    }

    public abstract class CrudControllerBase<TEntityDto, TPrimaryKey, TGetAllInput>
        : CrudControllerBase<TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected CrudControllerBase(
            ICrudService<TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto> service) : base(service)
        {
        }
    }

    public abstract class CrudControllerBase<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput>
        : CrudControllerBase<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TCreateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
    {
        protected CrudControllerBase(
            ICrudService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TCreateInput> service) : base(service)
        {
        }
    }

    [ApiController]
    [Produces("application/json")]
    public abstract class CrudControllerBase<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
        : QueryControllerBase<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        protected CrudControllerBase(
            ICrudService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput> service) : base(service)
        {
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] TCreateInput input)
        {
            var dto = await Service.CreateAsync(input);
            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(TPrimaryKey id, [FromBody] TUpdateInput input)
        {
            input.Id = id;
            await Service.UpdateAsync(input);
            return Ok();
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(TPrimaryKey id)
        {
            await Service.DeleteAsync(id);
            return Ok();
        }

        /// <summary>
        /// Bulk delete
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost("delete")]
        public async Task<IActionResult> Delete(IList<TPrimaryKey> ids)
        {
            await Service.DeleteAsync(ids);
            return Ok();
        }
    }
}
