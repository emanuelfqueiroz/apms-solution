using System.ComponentModel;

namespace AffiliatePMS.Domain.Common
{
    public enum Gender
    {
        [Description("Male")]
        Male,
        [Description("Female")]
        Female,
        [Description("Other")]
        Other
    }

    public static class GenderEnum
    {
        public static Dictionary<int, string> Genders { get; private set; }

        static GenderEnum()
        {
            GenderEnum.Genders = new Dictionary<int, string>
            {
                { (int) Gender.Male, "Male" },
                { (int) Gender.Female, "Female" },
                { (int)Gender.Other, "Other" }
            };
        }
        public static string GetDescription(this Gender gender)
        {
            return GenderEnum
                .Genders
                .Single(g => gender.Equals(g.Key))
                .Value;
        }
    }
}
