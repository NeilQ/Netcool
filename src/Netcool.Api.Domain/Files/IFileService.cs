using System.Collections.Generic;
using Netcool.Core.Services;

namespace Netcool.Api.Domain.Files
{
    public interface IFileService : ICrudService<FileDto, int, FileQuery, FileSaveInput>
    {
        void Active(FileActiveInput input);
        void Active(List<int> ids, string description);
    }
}
