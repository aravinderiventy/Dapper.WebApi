namespace Dapper.Application.Repositories
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
    }
}
