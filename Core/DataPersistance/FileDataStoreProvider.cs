namespace Core.DataPersistance
{
    public interface IDataStoreProvider
    {
        public Task<string> GetLastKnownIpAddress(string filePath, CancellationToken token);

        public Task UpdateIpAddress(string filePath, string address);
    }

    public class FileDataStoreProvider : IDataStoreProvider
    {
        
        public async Task<string> GetLastKnownIpAddress(string filePath, CancellationToken token )
        {
            if (!File.Exists(filePath)) return null;

            var fileLines = await File.ReadAllLinesAsync(filePath, token);
            return fileLines.First();
        }

        public async Task UpdateIpAddress(string filePath, string address)
        {
            using (var writer = new StreamWriter(filePath, false))
            {
                await writer.WriteLineAsync(address);
            }
        }
    }
}
