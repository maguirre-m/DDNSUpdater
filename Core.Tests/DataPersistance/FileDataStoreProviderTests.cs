using Core.DataPersistance;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Tests.DataPersistance
{
    public class FileDataStoreProviderTests
    {
        private const string DDNSUpdaterFileName = "IPAddressInfo.txt";

        [Fact]
        public async void ShouldReturnNullWhenNothingHasBeenSaved()
        {
            File.Delete(DDNSUpdaterFileName);
            var provider = new FileDataStoreProvider();

            var lastKnownAddress = await provider.GetLastKnownIpAddress(DDNSUpdaterFileName, CancellationToken.None);

            lastKnownAddress.ShouldBeNull();
        }

        [Fact]
        public async void ShouldUpdateLastKnownIPAddress()
        {
            File.Delete(DDNSUpdaterFileName);
            var provider = new FileDataStoreProvider();

            var ipAddress = "8.8.8.8";

            await provider.UpdateIpAddress(DDNSUpdaterFileName, ipAddress);

            var lastKnownAddress = await provider.GetLastKnownIpAddress(DDNSUpdaterFileName, CancellationToken.None);

            lastKnownAddress.ShouldBe(DDNSUpdaterFileName, ipAddress);

            ipAddress = "127.0.0.1";

            await provider.UpdateIpAddress(DDNSUpdaterFileName, ipAddress);

            lastKnownAddress = await provider.GetLastKnownIpAddress(DDNSUpdaterFileName, CancellationToken.None);

            lastKnownAddress.ShouldBe(ipAddress);
        }
    }
}
