using System.Collections.Generic;
using System.Threading.Tasks;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.Business.Interfaces
{
    public interface ISecuritySettingParserService
    {
        Task<List<SecuritySettingsParsed>> ParseSecuritySettings(int refId, string type);
    }
}
