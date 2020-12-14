using Netcool.Core.Services;

namespace Netcool.Api.Domain.Files
{
    public interface IFileService : ICrudService<FileDto, int, FileQuery, FileSaveInput>
    {
        void ActivePicture(FileActiveInput input);
    }
}