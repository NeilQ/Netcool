using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Netcool.Core.Repositories;
using Netcool.Core.Services;

namespace Netcool.Core.Announcements
{
    public interface IUserAnnouncementService : ICrudService<UserAnnouncementDto, int, UserAnnouncementRequest>
    {
        public Task ReadAsync(UserAnnouncementReadInput input);
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
            var query = Repository.GetQueryable()
                .Include(t => t.Announcement)
                .Where(t => t.Announcement.Status == AnnouncementStatus.Published);
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

        protected override IQueryable<UserAnnouncement> ApplySort(IQueryable<UserAnnouncement> query,
            UserAnnouncementRequest input)
        {
            return query.OrderBy(t => t.Announcement.UpdateTime);
        }

        public async Task ReadAsync(UserAnnouncementReadInput input)
        {
            if (input.AnnouncementIds == null || input.AnnouncementIds.Count == 0) return;
            var uas = Repository.GetQueryable()
                .Include(t => t.Announcement)
                .Where(t => input.AnnouncementIds.Contains(t.Announcement.Id) &&
                            t.Announcement.Status == AnnouncementStatus.Published)
                .Where(t => t.UserId == input.UserId && !t.IsRead).ToList();
            foreach (var ua in uas)
            {
                ua.IsRead = true;
            }

            await UnitOfWork.SaveChangesAsync();
        }
    }
}
