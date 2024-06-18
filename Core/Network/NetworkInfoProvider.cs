using System.Net;

namespace Core.Network
{
    public interface INetworkInfoProvider
    {
        public Task<string> GetCurrentPublicIPAddress();
    }

    public class NetworkInfoProvider : INetworkInfoProvider
    {
        public async Task<string> GetCurrentPublicIPAddress()
        {
            using (var client = new HttpClient())
            {
                var externalIPAddress = await client.GetStringAsync("https://ipinfo.io/ip");
                if (IPAddress.TryParse(externalIPAddress.Trim(), out var iPAddress))
                {
                    return iPAddress.ToString();
                }
                return string.Empty;
            }
        }
    }
}
