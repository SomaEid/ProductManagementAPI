using ProductManagementSolution.Application.Interfaces;
using ProductManagementSolution.Domain.Entities;
using ProductManagementSolution.Infrastructure.Data.Repositories;

namespace ProductManagementSolution.Infrastructure.Data
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IGenericRepository<Product> Products { get; private set; }
        public IGenericRepository<Item> Items { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Products = new GenericRepository<Product>(_context);
            Items = new GenericRepository<Item>(_context);
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
