using System.Threading.Tasks;
using Netcool.Core.Services;

namespace Netcool.Core.Announcements
{
    public interface IAnnouncementService :
        ICrudService<AnnouncementDto, int, AnnouncementRequest, AnnouncementSaveInput>
    {
        Task PublishAsync(int id);
    }
}
