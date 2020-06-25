using System.Collections.Generic;
using System.Threading.Tasks;

namespace Readinizer.Backend.DataAccess.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAllEntities();
        void Add(TEntity entity);
        void AddRange(List<TEntity> entities);
        void Update(TEntity entityToUpdate);
        void DeleteById(object id);
        void Delete(TEntity entityToDelete);
        TEntity GetByID(object id);
    }
}
