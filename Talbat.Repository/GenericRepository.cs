﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Entities;
using Talbat.Core.Repositories.Contract;
using Talbat.Core.Specifications;
using Talbat.Repository.Data;

namespace Talbat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            //if(typeof(T)==typeof(Product))
            //    return (IEnumerable<T>) await _dbContext.Set<Product>().Include(p=>p.Brand).Include(p=>p.Category).ToListAsync();
            return await _dbContext.Set<T>().ToListAsync();
        }
        public async Task<T?> GetByIdAsync(int id)
        {
            //if(typeof(T) == typeof(Product))
            //    return await _dbContext.Set<Product>().Where(P => P.Id == id).Include(p => p.Brand).Include(p => p.Category).FirstOrDefaultAsync() as T;
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public async Task<IReadOnlyList<T>> GetAllSpecificationsAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).ToListAsync();
        }

        public async Task<T?> GetWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }
        public async Task<int> GetCountAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).CountAsync();
        }
        private IQueryable<T> ApplySpecifications(ISpecifications<T> spec)
        {
            return SpecificationsEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

        public async Task AddAsync(T entity)
            => await _dbContext.AddAsync(entity);

        public void Update(T entity)
            => _dbContext.Update(entity);

        public void Delete(T entity)
            => _dbContext.Remove(entity);
    }
}
