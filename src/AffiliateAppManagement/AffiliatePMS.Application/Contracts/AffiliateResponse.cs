namespace AffiliatePMS.Application.Contracts
{
    public record AffiliateResponse
    {
        public int Id { get; set; }
        public string? PublicName { get; set; }
        public List<SocialMedia>? SocialMedias { get; set; }

        public record SocialMedia
        {

            public int Id { get; set; }

            public string? Url { get; set; }

            public string? Type { get; set; }

            public int? Followers { get; set; }

        }
    }


}
