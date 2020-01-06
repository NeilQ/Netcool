namespace Netcool.Core.Services.Dto
{
    public interface IPageRequest
    {
        int Page { get; set; }

        int Size { get; set; }
        
        string Sort { get; set; }
    }
}