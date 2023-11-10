using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netcool.Core;
using Netcool.Core.Entities;
using Netcool.Core.Repositories;
using Netcool.Core.Services;

namespace Netcool.Api.Domain.Files
{
    public class FileService : CrudService<File, FileDto, int, FileQuery, FileSaveInput>, IFileService
    {
        private readonly FileUploadOptions _fileUploadOptions;
        private readonly ILogger _logger;

        public FileService(IRepository<File> repository,
            IServiceAggregator serviceAggregator,
            IOptions<FileUploadOptions> pictureOptionsAccessor,
            ILogger<FileService> logger) :
            base(repository, serviceAggregator)
        {
            _logger = logger;
            _fileUploadOptions = pictureOptionsAccessor.Value;
        }

        protected override IQueryable<File> CreateFilteredQuery(FileQuery input)
        {
            var query = base.CreateFilteredQuery(input);

            if (input.IsActive != null)
            {
                query = query.Where(t => t.IsActive == input.IsActive);
            }

            return query;
        }

        protected override FileDto MapToEntityDto(File entity)
        {
            var dto = base.MapToEntityDto(entity);
            return dto;
        }

        /// <summary>
        /// 将文件标记为有效
        /// </summary>
        /// <returns></returns>
        public async Task ActiveAsync(FileActiveInput input)
        {
            if (input == null) return;
            var file = Repository.Get(input.Id);
            if (file == null) throw new EntityNotFoundException(typeof(File), input.Id);
            file.Description = input.Description;
            file.IsActive = true;
            await Repository.UpdateAsync(file);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task ActiveAsync(List<int> ids, string description)
        {
            if (ids == null || ids.Count == 0) return;
            var files = Repository.GetAllList(t => ids.Contains(t.Id));
            if (files == null || files.Count == 0) return;
            foreach (var file in files)
            {
                file.Description = description;
                file.IsActive = true;
                await Repository.UpdateAsync(file);
            }

            await UnitOfWork.SaveChangesAsync();
        }

        public override async Task DeleteAsync(int id)
        {
            CheckDeletePermission();
            var file = Repository.Get(id);
            if (file == null) return;
            await base.DeleteAsync(id);

            // 物理文件删除
            var picturePath = _fileUploadOptions.PhysicalPath + "/" + file.Filename;
            try
            {
                if (System.IO.File.Exists(picturePath))
                {
                    System.IO.File.Delete(picturePath);
                }
            }
            catch (Exception e)
            {
                var message = $"物理文件删除失败:[{file.Id}:{file.Filename}]";
                _logger.LogError(message, e);
                throw new UserFriendlyException(message);
            }

            await UnitOfWork.SaveChangesAsync();
        }

        public override async Task DeleteAsync(IEnumerable<int> ids)
        {
            CheckDeletePermission();
            if (ids == null || !ids.Any()) return;
            var pictures = Repository.GetAll().AsNoTracking().Where(t => ids.Contains(t.Id)).ToList();
            if (pictures.Count == 0) return;
            await base.DeleteAsync(ids);

            foreach (var picture in pictures)
            {
                var picturePath = _fileUploadOptions.PhysicalPath + "/" + picture.Filename;
                try
                {
                    if (System.IO.File.Exists(picturePath)) System.IO.File.Delete(picturePath);
                }
                catch (Exception e)
                {
                    var message = $"物理文件删除失败:[{picture.Id}:{picture.Filename}]";
                    _logger.LogError(message, e);
                    throw new UserFriendlyException(message);
                }
            }

            await UnitOfWork.SaveChangesAsync();
        }
    }
}
