namespace Netcool.Api.Core.Entities
{
    public interface IEntity : IEntity<int>
    {

    }

    public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }

        bool IsNewEntity();
    }
}