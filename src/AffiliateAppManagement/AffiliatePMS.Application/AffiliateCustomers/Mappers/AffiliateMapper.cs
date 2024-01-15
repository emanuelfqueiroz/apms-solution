using AffiliatePMS.Application.Contracts;
using AffiliatePMS.Domain.Affiliates;
using Mapster;


namespace AffiliatePMS.Application.AffiliateCustomers.Mappers
{
    public static class AffiliateMapper
    {
        internal static void Load()
        {
            TypeAdapterConfig<Affiliate, AffiliateResponse>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.PublicName, src => src.PublicName)
                .Map(dest => dest.SocialMedias, src => src.AffiliateSocialMedia);

            TypeAdapterConfig<AffiliateSocialMedia, AffiliateResponse.SocialMedia>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Type, src => src.Type)
                .Map(dest => dest.Url, src => src.Url)
                .Map(dest => dest.Followers, src => src.Followers);
        }
    }
}
