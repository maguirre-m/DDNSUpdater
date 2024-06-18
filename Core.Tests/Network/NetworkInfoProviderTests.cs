using Core.Network;
using Shouldly;

namespace Core.Tests.Network
{
    public class NetworkInfoProviderTests
    {
        [Fact]
        public async void ShouldProvideAValidIPAddress()
        {
            var ipAddressProvider = new NetworkInfoProvider();
            var publicIP = await ipAddressProvider.GetCurrentPublicIPAddress();

            publicIP.ShouldNotBe(null);
            publicIP.ShouldNotBe(string.Empty);

            Console.WriteLine(publicIP.ToString());
        }
    }
}
