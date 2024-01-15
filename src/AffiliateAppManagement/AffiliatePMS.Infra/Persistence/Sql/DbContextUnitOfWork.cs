using AffiliatePMS.Application.Common;

namespace AffiliatePMS.Infra.Persistence.Sql
{
    internal class DbContextUnitOfWork(APMSDbContext dbContext) : IUnitOfWork
    {
        public Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            return dbContext.SaveChangesAsync();
        }
    }
}
