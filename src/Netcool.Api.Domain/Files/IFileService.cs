using System.Collections.Generic;
using System.Threading.Tasks;
using Netcool.Core.Services;

namespace Netcool.Api.Domain.Files
{
    public interface IFileService : ICrudService<FileDto, int, FileQuery, FileSaveInput>
    {
        Task ActiveAsync(FileActiveInput input);
        
        Task ActiveAsync(List<int> ids, string description);
    }
}
