namespace AffiliatePMS.Application.Contracts
{
    public class AdRealTimeStats
    {
        public int AdId { get; set; }
        public DateTime? EndTime { get; set; }
        public string? ProductName { get; set; }
        public int? ProductId { get; set; }
        public StatsData? Stats { get; set; }
        public class StatsData
        {
            public int TotalViewed { get; set; }
            public int TotalPurchases { get; set; }
            public decimal TotalPurchasesAmount { get; set; }

            public int? VisualizationsLastDay { get; set; }
            public int? PurchasesLastDay { get; set; }

        }
    }
}
