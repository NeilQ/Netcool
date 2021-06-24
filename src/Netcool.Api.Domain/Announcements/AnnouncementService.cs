using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        public AnnouncementService(IRepository<Announcement> repository,
            IServiceAggregator serviceAggregator,
            IUserRepository userRepository,
            IRepository<UserAnnouncement> userAnnouncementRepository) :
            base(repository, serviceAggregator)
        {
            _userRepository = userRepository;
            _userAnnouncementRepository = userAnnouncementRepository;
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

        public override AnnouncementDto Create(AnnouncementSaveInput input)
        {
            CheckCreatePermission();
            var entity = MapToEntity(input);
            entity.Status = AnnouncementStatus.Draft;

            Repository.Insert(entity);
            UnitOfWork.SaveChanges();

            return MapToEntityDto(entity);
        }

        public override AnnouncementDto Update(AnnouncementSaveInput input)
        {
            CheckUpdatePermission();
            var entity = GetEntityById(input.Id);
            if (entity.Status == AnnouncementStatus.Published)
            {
                throw new UserFriendlyException("该公告已发布，无法更新");
            }

            MapToEntity(input, entity);
            UnitOfWork.SaveChanges();

            return MapToEntityDto(entity);
        }

        public void Publish(int id)
        {
            CheckPermission(PublishPermissionName);

            var entity = GetEntityById(id);
            entity.Status = AnnouncementStatus.Published;
            Repository.Update(entity);
            var userIds = _userRepository.GetAll()
                .Where(t => t.IsActive == (entity.NotifyTargetType == NotifyTargetType.ActiveUsers))
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

            UnitOfWork.SaveChanges();
        }
    }
}
