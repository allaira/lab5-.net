using lab_5;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace lab_5
{
    public class JsonFileWriter : IFileSaver
    {
        public async Task SaveAsync<T>(string filePath, List<T> data)
        {
            string? directoryPath = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);

            await File.WriteAllTextAsync(filePath, json);
        }
    }
}