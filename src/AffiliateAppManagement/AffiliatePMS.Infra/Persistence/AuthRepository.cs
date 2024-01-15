using AffiliatePMS.Infra.Persistence.Common;
using AffiliatePMS.Infra.Security;
using AffiliatePMS.Infra.Security.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AffiliatePMS.Infra.Persistence;
internal class AuthRepository(IConfiguration config) : IAuthRepository
{
    public required IConfiguration Config { get; set; } = config;
    private string connectionString => Config.GetConnectionString("APMSConnection")!;

    public async Task<UserAuth?> AddUserAsync(string name, string email, string encodedPassword, int? affiliateId)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();

            var command = new SqlCommand("INSERT INTO [AppUser] (FullName, Email, EncodedPassword, Status, Role) VALUES (@fullname, @email, @encoded, @status, @role); SELECT SCOPE_IDENTITY();", connection);

            var (role, status) = (Role.Incomplete, UserStatus.Active);

            command.Parameters.AddWithValue("@fullname", name);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@encoded", encodedPassword);
            command.Parameters.AddWithValue("@role", role);
            command.Parameters.AddWithValue("@status", (int)status);

            var result = await command.ExecuteScalarAsync();

            if (result != null)
            {
                return new UserAuth(
                    id: Convert.ToInt32(result),
                    name: name,
                    email: email,
                    encodedPassword: encodedPassword,
                    status: status,
                    role: role,
                    affiliateId: affiliateId
                );
            }
        }

        return null;
    }

    public async Task<UserAuth?> GetUserAsync(string email)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand("SELECT Id, FullName, Email, EncodedPassword, [Status], [Role], AffiliateId FROM [AppUser] WHERE Email = @email", connection);
            command.Parameters.AddWithValue("@email", email);

            await connection.OpenAsync();

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new UserAuth(

                        id: reader.GetInt32(reader.GetOrdinal("Id")),
                        name: reader.GetString(reader.GetOrdinal("FullName")),
                        email: reader.GetString(reader.GetOrdinal("Email")),
                        encodedPassword: reader.GetString(reader.GetOrdinal("EncodedPassword")),
                        status: (UserStatus)reader.GetInt16(reader.GetOrdinal("Status")),
                        role: reader.GetString(reader.GetOrdinal("Role")),
                        affiliateId: reader.GetNullableInt("AffiliateId")
                    );

                }
            }
        }

        return null;
    }
}