using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace AffiliatePMS.Infra.Persistence.Common
{
    public static class SqlReaderExtension
    {
        public static int? GetNullableInt(this SqlDataReader reader, string name)
        {
            var pos = reader.GetOrdinal(name);
            return reader.IsDBNull(pos) ? null : reader.GetInt32(pos);
        }
        public static string? GetSafeString(this SqlDataReader reader, string name)
        {
            var pos = reader.GetOrdinal(name);
            return reader.IsDBNull(pos) ? null : reader.GetString(pos);
        }

        public static List<T> GetStructuredData<T>(this SqlDataReader reader, string name) where T : class
        {
            var pos = reader.GetOrdinal(name);
            return reader.IsDBNull(pos) ? new List<T>() : JsonSerializer.Deserialize<List<T>>(reader.GetString(pos))!;
        }
    }
}
