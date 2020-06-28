using System;
using System.Threading.Tasks;

namespace Readinizer.Backend.Business.Interfaces
{
    public interface IExportService
    {
        Task<bool> Export(Type type, string path);
    }
}
