namespace AffiliatePMS.Application.Common
{

    public interface IUnitOfWorkADO
    {
        void Commit();
        void Rollback();
        void BeginTransaction();
    }

    public interface IUnitOfWork
    {
        Task<int> SaveAsync(CancellationToken cancellationToken = default);
    }
}
