using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// 将图片设置为有效
        /// </summary>
        /// <returns></returns>
        public void ActivePicture(FileActiveInput input)
        {
            if (input == null) return;
            var picture = Repository.Get(input.Id);
            if (picture == null) throw new EntityNotFoundException(typeof(File), input.Id);
            picture.Description = input.Description;
            picture.IsActive = true;
            Repository.Update(picture);
            UnitOfWork.SaveChanges();
        }

        public override void Delete(int id)
        {
            CheckDeletePermission();
            var picture = Repository.Get(id);
            if (picture == null) return;
            base.Delete(id);

            // 物理文件删除
            var picturePath = _fileUploadOptions.PhysicalPath + "/" + picture.Filename;
            try
            {
                if (System.IO.File.Exists(picturePath))
                {
                    System.IO.File.Delete(picturePath);
                }
            }
            catch (Exception e)
            {
                var message = $"物理文件删除失败:[{picture.Id}:{picture.Filename}]";
                _logger.LogError(message, e);
                throw new UserFriendlyException(message);
            }

            UnitOfWork.SaveChanges();
        }

        public override void Delete(IEnumerable<int> ids)
        {
            CheckDeletePermission();
            if (ids == null || !ids.Any()) return;
            var pictures = Repository.GetAll().AsNoTracking().Where(t => ids.Contains(t.Id)).ToList();
            if (pictures.Count == 0) return;
            base.Delete(ids);

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

            UnitOfWork.SaveChanges();
        }
    }
}