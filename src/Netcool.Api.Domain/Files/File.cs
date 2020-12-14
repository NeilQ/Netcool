using Netcool.Core.Entities;

namespace Netcool.Api.Domain.Files
{
    /// <summary>
    /// 文件
    /// </summary>
    public class File : FullAuditEntity
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

        /// <summary>
        /// 是否有效
        /// 刚上传的文件为false, 文件与其他表关联后设置为true
        /// 如有自动清理文件的job，会根据此字段清理无效文件
        /// </summary>
        public bool IsActive { get; set; }
    }
}