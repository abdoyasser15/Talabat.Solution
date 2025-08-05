using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core;
using Talbat.Core.Entities;
using Talbat.Core.Entities.Order_Aggregate;
using Talbat.Core.Repositories.Contract;
using Talbat.Repository.Data;

namespace Talbat.Repository
{
    public class UnitOfWork : IUntiOfWork
    {
        private readonly StoreContext _dbContext;
        //private Dictionary<string, GenericRepository<BaseEntity>> _repositories;
        private Hashtable _repositories;

        public UnitOfWork(StoreContext dbContext) // Ask CLR TO Creating Object From DbContext Implecitly
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var key = typeof(TEntity).Name; // Order
            if (!_repositories.ContainsKey(key))
            {
                var repository = new GenericRepository<TEntity>(_dbContext);

                _repositories.Add(key, repository);
            }
            return _repositories[key] as GenericRepository<TEntity>;
        }
        public async Task<int> CompleteAsync()
            => await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
            => await _dbContext.DisposeAsync();
        

    }
}
