using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Readinizer.Backend.DataAccess.Context;
using Readinizer.Backend.DataAccess.Interfaces;

namespace Readinizer.Backend.DataAccess.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity: class
    {
        internal ReadinizerDbContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(ReadinizerDbContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual Task<List<TEntity>> GetAllEntities()
        {
            return dbSet.ToListAsync();
        }

        public virtual void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            dbSet.Add(entity);
        }

        public virtual void AddRange(List<TEntity> entities)
        {
            dbSet.AddRange(entities);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void DeleteById(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }

            dbSet.Remove(entityToDelete);
        }
    }
}
