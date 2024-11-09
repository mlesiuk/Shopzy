using Microsoft.EntityFrameworkCore;
using Shopzy.Application.Abstractions.Interfaces;
using Shopzy.Domain.Entities;
using Shopzy.Infrastructure.Persistence;

namespace Shopzy.Infrastructure.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context) : base(context)
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
