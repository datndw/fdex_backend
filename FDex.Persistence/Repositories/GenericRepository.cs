using System;
using FDex.Application.Contracts.Persistence;
using FDex.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace FDex.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
		private readonly FDexDbContext _context;

		public GenericRepository(FDexDbContext context)
		{
			_context = context;
		}

        public async Task<T> AddAsync(T entity)
        {
            await _context.AddAsync(entity);
            return entity;
        }

        public async Task<T> FindAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}

