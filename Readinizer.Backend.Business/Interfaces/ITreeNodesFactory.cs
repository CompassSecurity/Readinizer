using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.Business.Interfaces
{
    public interface ITreeNodesFactory
    {
        Task<ObservableCollection<TreeNode>> CreateTree();
    }
}
