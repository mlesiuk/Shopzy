using Microsoft.EntityFrameworkCore;
using Shopzy.Application.Abstractions.Interfaces;
using Shopzy.Domain.Entities;
using Shopzy.Infrastructure.Persistence;

namespace Shopzy.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : AuditableEntity
{
    private readonly ApplicationDbContext _context;

    public BaseRepository(ApplicationDbContext dbContext)
    {
        _context = dbContext;
    }

    public void Add(TEntity entity)
    {
        _context
            .Set<TEntity>()
            .Add(entity);
    }

    public void Delete(TEntity entity)
    {
        _context
            .Set<TEntity>()
            .Remove(entity);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, bool asNoTracking = true)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();
        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.SingleOrDefaultAsync(x => x.Id == id);
    }

    public void Update(TEntity entity)
    {
        _context
            .Set<TEntity>()
            .Update(entity);
    }
}
