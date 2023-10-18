using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Netcool.Api.Domain.Files;
using Netcool.Api.Domain.Users;
using Netcool.Core.Repositories;
using Netcool.Core.Services;

namespace Netcool.Core.Announcements
{
    public sealed class AnnouncementService :
        CrudService<Announcement, AnnouncementDto, int, AnnouncementRequest, AnnouncementSaveInput>,
        IAnnouncementService
    {
        private const string PublishPermissionName = "announcement.publish";

        private readonly IUserRepository _userRepository;
        private readonly IRepository<UserAnnouncement> _userAnnouncementRepository;
        private readonly IFileService _fileService;

        public AnnouncementService(IRepository<Announcement> repository,
            IServiceAggregator serviceAggregator,
            IUserRepository userRepository,
            IRepository<UserAnnouncement> userAnnouncementRepository,
            IFileService fileService) :
            base(repository, serviceAggregator)
        {
            _userRepository = userRepository;
            _userAnnouncementRepository = userAnnouncementRepository;
            _fileService = fileService;
            GetPermissionName = "announcement.view";
            UpdatePermissionName = "announcement.update";
            CreatePermissionName = "announcement.create";
            DeletePermissionName = "announcement.delete";
        }

        protected override IQueryable<Announcement> CreateFilteredQuery(AnnouncementRequest input)
        {
            var query = base.CreateFilteredQuery(input);
            if (!string.IsNullOrEmpty(input.Title))
            {
                query = query.Where(t => t.Title.Contains(input.Title));
            }

            if (input.Status != null)
            {
                query = query.Where(t => t.Status == input.Status.Value);
            }

            if (input.NotifyTargetType != null)
            {
                query = query.Where(t => t.NotifyTargetType == input.NotifyTargetType.Value);
            }

            return query;
        }

        public override async Task<AnnouncementDto> CreateAsync(AnnouncementSaveInput input)
        {
            CheckCreatePermission();
            var entity = MapToEntity(input);
            entity.Status = AnnouncementStatus.Draft;

            if (!string.IsNullOrEmpty(input.Body))
            {
                var fileIds = FetchFileIds(input.Body);
                await _fileService.ActiveAsync(fileIds, $"公告[{input.Title}]插图");
            }

            Repository.Insert(entity);
            await UnitOfWork.SaveChangesAsync();

            return MapToEntityDto(entity);
        }

        public override async Task<AnnouncementDto> UpdateAsync(AnnouncementSaveInput input)
        {
            CheckUpdatePermission();
            var entity = GetEntityById(input.Id);
            if (entity.Status == AnnouncementStatus.Published)
            {
                throw new UserFriendlyException("该公告已发布，无法更新");
            }

            var originFileIds = FetchFileIds(entity.Body);
            var currentFileIds = FetchFileIds(input.Body);

            using var scope = UnitOfWork.BeginTransactionScope();
            var fileIdsToDelete = originFileIds.Except(currentFileIds);
            var filesToAdd = currentFileIds.Except(originFileIds);
            await _fileService.DeleteAsync(fileIdsToDelete);
            await _fileService.ActiveAsync(filesToAdd.ToList(), $"公告[{input.Title}]插图");

            MapToEntity(input, entity);
            await UnitOfWork.SaveChangesAsync();
            scope.Complete();

            return MapToEntityDto(entity);
        }

        public override async Task DeleteAsync(int id)
        {
            var entity = GetEntityById(id);
            if (entity == null) return;
            var fileIds = FetchFileIds(entity.Body);

            using var scope = new TransactionScope();
            await _fileService.DeleteAsync(fileIds);
            await Repository.DeleteAsync(entity);
            await UnitOfWork.SaveChangesAsync();
            scope.Complete();
        }

        public override async Task DeleteAsync(IEnumerable<int> ids)
        {
            var entities = Repository.GetAllList(t => ids.Contains(t.Id));
            if (entities == null || entities.Count == 0) return;

            using var scope = UnitOfWork.BeginTransactionScope();
            foreach (var entity in entities)
            {
                var fileIds = FetchFileIds(entity.Body);
                await _fileService.DeleteAsync(fileIds);
            }

            await Repository.DeleteAsync(entities);
            await UnitOfWork.SaveChangesAsync();
            scope.Complete();
        }

        public async Task PublishAsync(int id)
        {
            CheckPermission(PublishPermissionName);

            var entity = GetEntityById(id);
            if (entity.Status == AnnouncementStatus.Published)
            {
                throw new UserFriendlyException("该公告已发布");
            }

            entity.Status = AnnouncementStatus.Published;
            Repository.Update(entity);
            await _userAnnouncementRepository.DeleteAsync(t => t.AnnouncementId == entity.Id);
            var userIds = _userRepository.GetAll()
                .AsNoTracking()
                .Select(t => t.Id).ToList();
            if (userIds.Count > 0)
            {
                var userAnnouncements = new List<UserAnnouncement>();
                foreach (var userId in userIds)
                {
                    userAnnouncements.Add(new UserAnnouncement()
                    {
                        UserId = userId,
                        AnnouncementId = id
                    });
                }

                _userAnnouncementRepository.Insert(userAnnouncements);
            }

            await UnitOfWork.SaveChangesAsync();
        }

        private List<int> FetchFileIds(string html)
        {
            var fileIds = new List<int>();
            if (string.IsNullOrEmpty(html)) return fileIds;

            var doc = new HtmlDocument();
            doc.LoadHtml(html); //or doc.Load(htmlFileStream)
            var nodes = doc.DocumentNode.SelectNodes(@"//img[@src]");
            if (nodes == null || nodes.Count == 0) return fileIds;
            foreach (var img in nodes)
            {
                string src = img.GetAttributeValue("src", null);
                if (!src.IsValidUrl()) continue;
                var uriBuilder = new UriBuilder(src);
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                if (int.TryParse(query["id"], out var id))
                {
                    fileIds.Add(id);
                }
            }

            return fileIds;
        }
    }
}
