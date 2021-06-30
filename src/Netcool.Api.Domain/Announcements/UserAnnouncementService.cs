using System.Linq;
using Microsoft.EntityFrameworkCore;
using Netcool.Core.Repositories;
using Netcool.Core.Services;
using Netcool.Core.Services.Dto;

namespace Netcool.Core.Announcements
{
    public interface IUserAnnouncementService : ICrudService<UserAnnouncementDto, int, UserAnnouncementRequest>
    {
    }

    public class UserAnnouncementService :
        CrudService<UserAnnouncement, UserAnnouncementDto, int, UserAnnouncementRequest>,
        IUserAnnouncementService
    {
        public UserAnnouncementService(IRepository<UserAnnouncement> repository, IServiceAggregator serviceAggregator) :
            base(repository, serviceAggregator)
        {
        }

        protected override IQueryable<UserAnnouncement> CreateFilteredQuery(UserAnnouncementRequest input)
        {
            var query = Repository.GetAll()
                .Include(t => t.Announcement)
                .Where(t=>t.Announcement.Status== AnnouncementStatus.Published);
            if (input.UserId != null)
            {
                query = query.Where(t => t.UserId == input.UserId);
            }

            if (input.IsRead != null)
            {
                query = query.Where(t => t.IsRead == input.IsRead);
            }

            return query;
        }
    }
}
