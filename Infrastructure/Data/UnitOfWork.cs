using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;
        private Hashtable _repositories;

        public UnitOfWork(StoreContext context)
        {
            _context = context;
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if (_repositories == null) _repositories = new Hashtable();

            string entityType = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(entityType))
            {
                Type repType = typeof(GenericRepository<>);
                object repInstance = Activator.CreateInstance(repType.MakeGenericType(typeof(TEntity)), _context);

                _repositories.Add(entityType, repInstance);
            }

            return (IGenericRepository<TEntity>)_repositories[entityType];
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
