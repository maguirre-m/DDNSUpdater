using Core.DataPersistance;
using Core.Network;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;


namespace Updater
{
    public sealed class Startup
    {
        public static IConfigurationRoot AddConfiguration(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }

        public static HostApplicationBuilder CreateBuilder(string[] args)
        {
            var host = Host.CreateApplicationBuilder(args);

            return host;
        }

        public static void ConfigureServices(HostApplicationBuilder builder)
        {
            builder.Services.AddOptions();
            builder.Services.AddSingleton<IDataStoreProvider, FileDataStoreProvider>();
            builder.Services.AddSingleton<INetworkInfoProvider, NetworkInfoProvider>();
            var section = builder.Configuration.GetSection(SqlServerDataStoreOptions.SqlServerDataStore);
            builder.Services.Configure<SqlServerDataStoreOptions>(section);
            builder.Services.AddTransient<ISqlServerDataStoreProvider, SqlServerDataStoreProvider>();

            builder.Services.AddSerilog((services, config) =>
            {
                config.ReadFrom.Configuration(builder.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext();
            });
        }
}
}
    
