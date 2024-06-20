using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Abstractions;
using System.Data;

namespace Core.DataPersistance
{
    public class SqlServerDataStoreOptions
    {
        public const string SqlServerDataStore = "SqlServerDataStore";

        public string ConnectionString { get; set; }
    }

    public class CloudFlareBaseSettings
    {
        public string ZoneId { get; set; }
        public string ApiKey { get; set; }
    }

    public class CloudFlareSetting
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public bool Valid { get; set; }
    }

    public interface ISqlServerDataStoreProvider
    {
        public Task<CloudFlareBaseSettings> GetCloudFlareBaseSettings();
        public Task<string> GetLastKnownIpAddress();
        public Task<string> UpdateLastKnownIpAddress(string ipAddress);
    }

    public class SqlServerDataStoreProvider : ISqlServerDataStoreProvider
    {
        private readonly SqlServerDataStoreOptions _cloudFlareBaseSettings;
        private readonly ILogger<SqlServerDataStoreProvider> _logger;

        public SqlServerDataStoreProvider(IOptions<SqlServerDataStoreOptions> sqlProviderSettings, ILogger<SqlServerDataStoreProvider> logger)
        {
            _cloudFlareBaseSettings = sqlProviderSettings.Value;
            _logger = logger;
        }

        public async Task<CloudFlareBaseSettings> GetCloudFlareBaseSettings()
        {
            try
            {
                using (var connection = new SqlConnection(_cloudFlareBaseSettings.ConnectionString))
                {
                    var ZoneIdKey = "ZoneId";
                    var ZoneTokenKey = "ApiKey";

                    var settings = await connection.QueryAsync<CloudFlareSetting>($"SELECT  * FROM CloudFlareSettings WHERE Name IN ('{ZoneIdKey}', '{ZoneTokenKey}')");

                    var settingsCount = settings.Count();

                    if (settings != null && settingsCount != 2)
                    {
                       _logger.LogError("Expected to retrieve 2 CloudFlareSetting records but received {0} settings", settingsCount);
                        throw new DataException($"Invalid amount of CloudFlareSetting records retrieved: {settingsCount}");
                    }

                    var baseSettings = new CloudFlareBaseSettings();

                    foreach (var setting in settings)
                    {
                        if (setting.Name == ZoneIdKey)
                        {
                            baseSettings.ZoneId = setting.Value;
                        }
                        else
                        {
                            baseSettings.ApiKey = setting.Value;
                        }
                    }

                    return baseSettings;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while attempting to retrieve CloudFlareSettings from the database: ");
                throw;
            }
        }

        public Task<string> GetLastKnownIpAddress()
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateLastKnownIpAddress(string ipAddress)
        {
            throw new NotImplementedException();
        }
    }
}
