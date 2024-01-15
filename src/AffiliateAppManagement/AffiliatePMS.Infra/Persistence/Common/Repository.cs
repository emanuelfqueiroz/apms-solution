using AffiliatePMS.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AffiliatePMS.Infra.Persistence.Common
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly APMSDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(APMSDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public virtual async Task<T?> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync(params Expression<Func<T, bool>>[] predicate)
        {
            var query = _dbSet.AsQueryable();
            foreach (var expression in predicate)
            {
                query = _dbSet.Where(expression);
            }
            return await query.ToListAsync();
        }

        public async Task<int> CountAssync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).CountAsync();
        }
    }
}
