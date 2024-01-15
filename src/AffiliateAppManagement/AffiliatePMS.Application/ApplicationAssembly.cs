using AffiliatePMS.Application.AffiliateCustomers.Mappers;
using AffiliatePMS.Application.Affiliates.Mappers;

namespace AffiliatePMS.Application
{
    public static class ApplicationAssembly
    {
        public static void ConfigureMappers()
        {
            AffiliateMapper.Load();
            AffiliateCustomerMapper.Load();
        }
    }
}
