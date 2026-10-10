using lab_5;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace lab_5
{
    public class JsonFileReader : IFileLoader
    {
        public async Task<List<T>?> LoadAsync<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null; 
            }
            
            string json = await File.ReadAllTextAsync(filePath);

            return JsonSerializer.Deserialize<List<T>>(json);
        }
    }
}