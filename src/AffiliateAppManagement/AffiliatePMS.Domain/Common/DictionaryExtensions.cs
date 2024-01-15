namespace AffiliatePMS.Domain.Common
{
    public static class DictionaryExtensions
    {
        public static string ToText(this Dictionary<int, string> dictionary)
        {
            return string.Join(", ", dictionary.Select(x => $"{x.Key} - {x.Value}"));
        }
    }
}
