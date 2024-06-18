using Core.DataPersistance;
using Core.Network;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Updater
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Initializing...");
            var host = Host.CreateDefaultBuilder(args).ConfigureServices(services =>
            {
                services.AddSingleton<IDataStoreProvider, FileDataStoreProvider>();
                services.AddSingleton<INetworkInfoProvider, NetworkInfoProvider>();
            }).Build();

            var config = host.Services.GetService<IConfiguration>();

            var ipAddressFilePath = config.GetValue<string>(Globals.Settings.IPAddressStoragePath);

            Console.WriteLine("Retrieving last known IP address...");

            var storageProvider = host.Services.GetRequiredService<IDataStoreProvider>();

            var lastIp = await storageProvider.GetLastKnownIpAddress(ipAddressFilePath, CancellationToken.None);

            Console.WriteLine($"Last known IP address: {lastIp}");

            Console.WriteLine($"Retrieving current IP address...");

            var networkInfoProvider = host.Services.GetRequiredService<INetworkInfoProvider>();

            var currentIp = await networkInfoProvider.GetCurrentPublicIPAddress();
            Console.WriteLine($"Current IP address: {currentIp}");

            if(!string.Equals(currentIp, lastIp, StringComparison.OrdinalIgnoreCase))
            {

            }
            else
            {
                Console.WriteLine($"IP address unchanged, shutting down.");
            }
            Console.WriteLine($"Press any key to finish.");
            Console.ReadKey();

        }
    }
}
