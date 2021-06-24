using Netcool.Core.Services;

namespace Netcool.Core.Announcements
{
    public interface IAnnouncementService :
        ICrudService<AnnouncementDto, int, AnnouncementRequest, AnnouncementSaveInput>
    {
        void Publish(int id);
    }
}
