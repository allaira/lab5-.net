using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_5
{
    public interface IFileLoader
    {
        Task<List<T>?> LoadAsync<T>(string filePath);
    }
}
