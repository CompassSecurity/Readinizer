using System.Collections.Generic;
using System.Threading.Tasks;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.Business.Interfaces
{
    public interface IAnalysisService
    {
        Task<List<Rsop>> Analyse(string importPath);
    }
}
