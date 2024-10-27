namespace Shopzy.Application.Abstractions.Interfaces;

public interface IBaseRepository<TEntity>
{
    void Add(TEntity entity);
    void Delete(TEntity entity);
    Task<TEntity?> GetByIdAsync(Guid id, bool asNoTracking = true);
    void Update(TEntity entity);
}
