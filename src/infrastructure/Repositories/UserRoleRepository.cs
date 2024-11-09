using Microsoft.EntityFrameworkCore;
using Shopzy.Application.Abstractions.Interfaces;
using Shopzy.Domain.Entities;
using Shopzy.Infrastructure.Persistence;

namespace Shopzy.Infrastructure.Repositories;

internal class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
{
    private readonly DbSet<UserRole> _roles;

    public UserRoleRepository(ApplicationDbContext context) : base(context)
    {
        _roles = context.Set<UserRole>();
    }

    public async Task<IEnumerable<Role>> AssignRoleAsync(User user, Role role)
    {
        return await _roles
            .Where(ur => ur.Id == user.Id)
            .Include(ur => ur.Role)
            .Where(r => r != null)
            .Select(ur => new Role()
            {
                Name = ur.Role!.Name
            }).ToListAsync() ?? Enumerable.Empty<Role>();
    }
}
