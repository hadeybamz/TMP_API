using Microsoft.EntityFrameworkCore;
using TMP_API.Data;
using TMP_API.Repository.IRepository;

namespace TMP_API.Repository;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class, new()
{
    protected DataContext _dbContext { get; set; }

    public async Task<T> GetAsync(int id)
    {
        return await _dbContext.FindAsync<T>(id);
    }

    public IQueryable<T> Query()
    {
        return _dbContext.Set<T>().AsQueryable();
    }

    public async Task InsertAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task<T> DeleteAsync(int id)
    {
        var value = _dbContext.FindAsync<T>(id);
        _dbContext.Remove(value.Result);
        await _dbContext.SaveChangesAsync();
        return await value;
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }
}
