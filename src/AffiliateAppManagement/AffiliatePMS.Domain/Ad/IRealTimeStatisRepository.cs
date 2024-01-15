namespace AffiliatePMS.Application.Contracts
{
    /// <summary>
    /// Using real time database like Redis or MemoryDB
    /// </summary>
    public interface IRealTimeStatisRepository
    {
        public Task<List<AdRealTimeStats>> GetRealTimeStatsAsync(int affiliateId);
    }
}
