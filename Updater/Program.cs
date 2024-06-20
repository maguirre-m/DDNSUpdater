using Core.CloudFlare.DNSRecords;
using Core.DataPersistance;
using Core.Network;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Updater
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("Updater.log", rollingInterval: RollingInterval.Day)
                .CreateBootstrapLogger();

            Log.Information("Initializing...");

            var builder = Startup.CreateBuilder(args);

            Startup.ConfigureServices(builder);

            var host = builder.Build();

            var config = host.Services.GetService<IConfiguration>();

            var ipAddressFilePath = config.GetValue<string>(Globals.Settings.IPAddressStoragePath);

            Log.Information("Retrieving last known IP address...");

            var storageProvider = host.Services.GetRequiredService<IDataStoreProvider>();

            var lastIp = await storageProvider.GetLastKnownIpAddress(ipAddressFilePath, CancellationToken.None);

            Log.Information($"Last known IP address: {lastIp}");

            Log.Information($"Retrieving current IP address...");

            var networkInfoProvider = host.Services.GetRequiredService<INetworkInfoProvider>();

            var currentIp = await networkInfoProvider.GetCurrentPublicIPAddress();
            Log.Information($"Current IP address: {currentIp}");

            if(!string.Equals(currentIp, lastIp, StringComparison.OrdinalIgnoreCase))
            {
                Log.Information("New IP Address found, updating DNS record");
                var sqlDataStoreProvider = host.Services.GetRequiredService<ISqlServerDataStoreProvider>();
                var cloudFlareSettings = await sqlDataStoreProvider.GetCloudFlareBaseSettings();

                try
                {
                    using (var client = new HttpClient())
                    {
                        HttpRequestMessage listDnsRecordsRequest = new HttpRequestMessage(HttpMethod.Get, $"https://api.cloudflare.com/client/v4/zones/{cloudFlareSettings.ZoneId}/dns_records"){};

                        listDnsRecordsRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", cloudFlareSettings.ApiKey);
                        
                        var result = await client.SendAsync(listDnsRecordsRequest);

                        if (result.IsSuccessStatusCode)
                        {
                            // Update
                            var stringContent = await result.Content.ReadAsStringAsync();

                            var serializerOptions = new JsonSerializerOptions()
                            {
                                PropertyNameCaseInsensitive = true
                            };

                            var listDnsRecordsResponse = JsonSerializer.Deserialize<ListDNSRecordsResponse>(stringContent, serializerOptions);
                        }
                        else
                        {
                            var stringContent = await result.Content.ReadAsStringAsync();
                            Log.Error($"Failed to retrieve DNS records from CloudFlare, details: {stringContent}");
                        }
                    }
                }
                catch (Exception ex) {
                    Log.Error(ex, "Failed to retrieve DNS records from CloudFlare, details: ");
                }
            }
            else
            {
                Log.Information($"IP address unchanged, shutting down.");
            }
            Log.Information($"Press any key to finish.");
            Console.ReadKey();

        }
    }
}
