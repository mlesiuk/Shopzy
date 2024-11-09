using Microsoft.EntityFrameworkCore;
using Shopzy.Application.Abstractions.Interfaces;
using Shopzy.Domain.Entities;
using Shopzy.Infrastructure.Persistence;

namespace Shopzy.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly DbSet<User> _users;
    private readonly DbSet<UserRole> _usersRoles;

    public UserRepository(ApplicationDbContext context) : base(context)
    {
        _users = context.Set<User>();
        _usersRoles = context.Set<UserRole>();
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        return await _users
            .SingleOrDefaultAsync(entity => entity.Email == email);
    }

    public async Task<User?> FindByUsernameAsync(string username)
    {
        return await _users
            .SingleOrDefaultAsync(entity => entity.Username == username);
    }

    public async Task<User?> FindByUsernameOrEmailAsync(string username, string email)
    {
        return await _users
            .SingleOrDefaultAsync(entity => entity.Username == username || entity.Email == email);
    }

    public async Task<User?> FindByNameAsync(string name)
    {
        return await _users
            .SingleOrDefaultAsync(entity => entity.Name == name || entity.SurName == name);
    }

    public async Task<IEnumerable<Role>> GetRolesAsync(Guid id)
    {
        return await _usersRoles
            .Where(ur => ur.UserId == id)
            .Include(ur => ur.Role)
            .Where(r => r != null)
            .Select(ur => new Role()
            {
                Name = ur.Role!.Name
            }).ToListAsync() ?? Enumerable.Empty<Role>();
    }
}
