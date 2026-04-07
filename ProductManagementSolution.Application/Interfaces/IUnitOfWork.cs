using ProductManagementSolution.Domain.Entities;

namespace ProductManagementSolution.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Product> Products { get; }
        IGenericRepository<Item> Items { get; }
        Task<int> CompleteAsync();
    }
}
