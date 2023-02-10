namespace TMP_API.Repository.IRepository;

public interface IBaseRepository<T>
{
    Task<T> GetAsync(int id);
    IQueryable<T> Query();
    Task InsertAsync(T entity);
    Task UpdateAsync(T entity);
    Task<T> DeleteAsync(int id);
    Task Save();
}