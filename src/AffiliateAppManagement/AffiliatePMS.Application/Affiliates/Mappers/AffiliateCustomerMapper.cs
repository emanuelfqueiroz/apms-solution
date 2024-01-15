using AffiliatePMS.Application.Contracts;
using Mapster;


namespace AffiliatePMS.Application.Affiliates.Mappers
{
    public static class AffiliateCustomerMapper
    {
        internal static void Load()
        {
            TypeAdapterConfig<Domain.AffiliateCustomers.AffiliateCustomer, AffiliateCustomerResponse>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.BirthDate, src => src.BirthDate)
                .Map(dest => dest.GenderId, src => src.Gender)
                .Map(dest => dest.AvgTicket, src => src.AvgTicket)
                .Map(dest => dest.TotalPurchase, src => src.TotalPurchase);
        }
    }
}
