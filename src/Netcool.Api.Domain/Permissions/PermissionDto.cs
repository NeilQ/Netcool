using Netcool.Core.Helpers;
using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Permissions
{
    public class PermissionDto : EntityDto
    {
        public string Name { get; set; }
        
        public string Code { get; set; }
        
        public string Notes { get; set; }
        
        public PermissionType Type { get; set; }
        
        public int MenuId { get; set; }
        
        public string TypeDescription => Reflection.GetEnumDescription(Type);
    }
}