using Microsoft.EntityFrameworkCore;
using Shopzy.Application.Abstractions.Interfaces;
using Shopzy.Application.Data;
using Shopzy.Domain.Entities;

namespace Shopzy.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly IApplicationDbContext _context;

    public UserRepository(IApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        return await _context
            .Set<User>()
            .SingleOrDefaultAsync(entity => entity.Email == email);
    }

    public async Task<User?> FindByUsernameAsync(string username)
    {
        return await _context
            .Set<User>()
            .SingleOrDefaultAsync(entity => entity.Username == username);
    }

    public async Task<User?> FindByUsernameOrEmailAsync(string username, string email)
    {
        return await _context
            .Set<User>()
            .SingleOrDefaultAsync(entity => entity.Username == username || entity.Email == email);
    }

    public async Task<User?> FindByNameAsync(string name)
    {
        return await _context
            .Set<User>()
            .SingleOrDefaultAsync(entity => entity.Name == name || entity.SurName == name);
    }

    public async Task<IEnumerable<Role>> GetRolesAsync(Guid id)
    {
        return await _context.Set<UserRole>()
            .Where(ur => ur.UserId == id)
            .Include(ur => ur.Role)
            .Select(ur => new Role()
            {
                Name = ur.Role.Name
            }).ToListAsync() ?? Enumerable.Empty<Role>();
    }

    public async Task<IEnumerable<Role>> AssignRoleAsync(User user, Role role)
    {
        return await _context.Set<User>()
            .Where(ur => ur.Id == user.Id)
            .Include(ur => ur.Roles)
            .Select(ur => new Role()
            {
                Name = ur.Name
            }).ToListAsync() ?? Enumerable.Empty<Role>();
    }
}
