using Microsoft.EntityFrameworkCore;
using Shopzy.Application.Abstractions.Interfaces;
using Shopzy.Domain.Entities;
using Shopzy.Infrastructure.Persistence;

namespace Shopzy.Infrastructure.Repositories;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    private readonly ApplicationDbContext _context;

    public RoleRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Role?> FindByNameAsync(string name)
    {
        return await _context
            .Set<Role>()
            .SingleOrDefaultAsync(entity => entity.Name == name);
    }
}
