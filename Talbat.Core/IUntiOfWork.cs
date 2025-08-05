using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Entities;
using Talbat.Core.Entities.Order_Aggregate;
using Talbat.Core.Repositories.Contract;

namespace Talbat.Core
{
    public interface IUntiOfWork : IAsyncDisposable 
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        Task<int> CompleteAsync();
    }
}
