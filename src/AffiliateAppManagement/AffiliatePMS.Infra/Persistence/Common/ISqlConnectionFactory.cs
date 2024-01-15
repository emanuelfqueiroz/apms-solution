using Microsoft.Data.SqlClient;

namespace AffiliatePMS.Infra.Persistence.Common
{
    public interface ISqlConnectionFactory
    {
        SqlConnection GetConnection();

        SqlCommand CreateCommand(string query);
    }
}