using AffiliatePMS.Application.Contracts;

namespace AffiliatePMS.Infra.Persistence.RealTime
{
    public class RedisCacheRepository : IRealTimeStatisRepository
    {
        private Task<string[]> GetActiveAdsFromSetsAsync(int affiliateId)
        {
            return Task.FromResult(new string[] { "1", "2", "3" });
        }
        public async Task<List<AdRealTimeStats>> GetRealTimeStatsAsync(int affiliateId)
        {
            string[] activeAdIds = await GetActiveAdsFromSetsAsync(affiliateId);
            return Enumerable.Range(1, 10).Select(index => new AdRealTimeStats()
            {
                AdId = index,
                EndTime = DateTime.Now.AddDays(index),
                ProductId = index,
                ProductName = $"Product {index}",
                Stats = new AdRealTimeStats.StatsData()
                {
                    PurchasesLastDay = index,
                    TotalPurchases = index,
                    TotalPurchasesAmount = index,
                    TotalViewed = index,
                    VisualizationsLastDay = index
                }
            }).ToList();
        }


    }
}
