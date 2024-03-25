using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace NourishNet.Repository.Repositories.Interfaces;

public class BaseRepository<T> : IRepository<T> where T : class
{
    protected readonly DbContext _context;
    private DbSet<T> _entities;

    public BaseRepository(DbContext context)
    {
        _context = context;
        _entities = context.Set<T>();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _entities.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _entities.ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _entities.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _entities.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _entities.AddRangeAsync(entities);
    }

    public void Remove(T entity)
    {
        _entities.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _entities.RemoveRange(entities);
    }

    public async Task UpdateAsync(T entity)
    {
        _entities.Update(entity);
        await Task.CompletedTask;
    }
}
