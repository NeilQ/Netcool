namespace Netcool.Core.Services.Dto
{
    public class PageRequest : IPageRequest
    {
        public int? Page { get; set; }
        public int? Size { get; set; }
        public string Sort { get; set; }
    }
}