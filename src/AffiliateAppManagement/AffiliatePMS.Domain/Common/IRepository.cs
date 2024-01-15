using System.Linq.Expressions;

namespace AffiliatePMS.Domain.Common
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> GetAll();
        Task<T?> GetAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(params Expression<Func<T, bool>>[] expressions);
        Task<int> CountAssync(Expression<Func<T, bool>> predicate);
    }
}
