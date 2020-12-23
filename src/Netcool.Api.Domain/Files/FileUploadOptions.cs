using Netcool.Core.Extensions;

namespace Netcool.Api.Domain.Files
{
    public class FileUploadOptions
    {
        private string _subWebPath;

        public static FileUploadOptions Instance { get; } = new FileUploadOptions();
        
        public string HostSchema { get; set; }

        /// <summary>
        /// http host, 如 www.domain.com
        /// </summary>
        public string Host { get; set; }
     
        /// <summary>
        /// host后面的相对web路径 
        /// </summary>
        public string SubWebPath
        {
            get => string.IsNullOrWhiteSpace(_subWebPath) ? "file" : _subWebPath.Trim('/');
            set => _subWebPath = value;
        }

        /// <summary>
        /// 文件物理地址
        /// </summary>
        public string PhysicalPath { get; set; }
    }
}