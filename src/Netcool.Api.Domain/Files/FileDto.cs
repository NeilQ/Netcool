using System.ComponentModel.DataAnnotations;
using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Files
{
    public class FileSaveInput : EntityDto
    {
        /// <summary>
        /// 标题, 通常是保存提交的原始文件名
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 文件名(相对位置，去除host)
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// 描述，表达文件的用处
        /// </summary>
        public string Description { get; set; }
    }

    public class FileDto : FileSaveInput
    {
        /// <summary>
        /// 绝对url
        /// </summary>
        public string Url { get; set; }

        public string Host { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsActive { get; set; }
    }

    public class FileQuery : PageRequest
    {
        public bool? IsActive { get; set; }
    }

    public class PictureBase64Upload
    {
        public string Filename { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Base64 { get; set; }

        [MaxLength(512)]
        public string Description { get; set; }

        /// <summary>
        /// 是否保持原文件名
        /// </summary>
        public bool KeepFileName { get; set; }
    }

    public class FileActiveInput
    {
        /// <summary>
        /// 图片id
        /// </summary>
        public int Id { get; set; }

        [MaxLength(512)]
        public string Description { get; set; }
    }
}