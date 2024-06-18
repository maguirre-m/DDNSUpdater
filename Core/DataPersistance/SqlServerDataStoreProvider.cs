using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

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

        public SqlServerDataStoreProvider(IOptions<SqlServerDataStoreOptions> sqlProviderSettings)
        {
            _cloudFlareBaseSettings = sqlProviderSettings.Value;
        }

        public async Task<CloudFlareBaseSettings> GetCloudFlareBaseSettings()
        {
            using (var connection = new SqlConnection(_cloudFlareBaseSettings.ConnectionString))
            {
                var ZoneIdKey = "ZoneId";
                var ZoneTokenKey = "ApiKey";

                var settings = await connection.QueryAsync<CloudFlareSetting>($"SELECT  * FROM CloudFlareSettings WHERE Name IN ('{ZoneIdKey}', '{ZoneTokenKey}')");

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
