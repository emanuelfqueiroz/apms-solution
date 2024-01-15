using AffiliatePMS.Domain.Affiliates;
using Microsoft.EntityFrameworkCore;

namespace AffiliatePMS.Infra.Persistence;

//[assembly: InternalsVisibleTo("AffiliatePMS.Infra.Tests")]
public class AffiliateRepository(APMSDbContext db) : Common.Repository<Affiliate>(db), IAffiliateRepository
{
    public override IQueryable<Affiliate> GetAll()
    {
        return db
            .Affiliates
            .Include(p => p.AffiliateSocialMedia);
    }

    public override Task<List<Affiliate>> GetAllAsync()
    {
        return GetAll().ToListAsync();
    }
    public async Task<int> TotalAsync()
    {
        return await db.Affiliates.CountAsync();
    }
    public override async Task<Affiliate?> GetAsync(int id)
    {
        return await db.Affiliates
            .Include(p => p.AffiliateSocialMedia)
            .FirstAsync(p => p.Id == id);
    }

    public async Task<bool> IsEmailUsedAsync(string email)
    {
        return await db.AffiliateDetails.AnyAsync(p => p.Email == email);
    }
}
