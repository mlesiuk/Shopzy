using Microsoft.EntityFrameworkCore;
using Shopzy.Application.Abstractions.Interfaces;
using Shopzy.Application.Data;
using Shopzy.Domain.Entities;

namespace Shopzy.Infrastructure.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    private readonly IApplicationDbContext _context;

    public CategoryRepository(IApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Category?> FindByNameAsync(string name)
    {
        return await _context
            .Set<Category>()
            .SingleOrDefaultAsync(entity => entity.Name == name);
    }
}
