using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Updater
{
    public class Startup
    {
        public static IConfigurationRoot AddConfiguration(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            // Add functionality to inject IOptions<T>
            services.AddOptions();

            // Add our Config object so it can be injected
        }
    }
}
    
