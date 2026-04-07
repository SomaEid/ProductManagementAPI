using System.Linq.Expressions;

namespace ProductManagementSolution.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> Find(Expression<Func<T, bool>> expression, bool trackChanges = false);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
