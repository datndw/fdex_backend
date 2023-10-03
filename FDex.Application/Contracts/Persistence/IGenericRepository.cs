using System;
namespace FDex.Application.Contracts.Persistence
{
	public interface IGenericRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task<T> FindAsync(string id);
        void Remove(T entity);
        void Update(T entity);
    }
}

