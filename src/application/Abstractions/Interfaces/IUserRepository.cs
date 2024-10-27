using Shopzy.Domain.Entities;

namespace Shopzy.Application.Abstractions.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> FindByNameAsync(string name);
    Task<User?> FindByEmailAsync(string email);
    Task<User?> FindByUsernameAsync(string username);
    Task<User?> FindByUsernameOrEmailAsync(string name, string email);
    Task<IEnumerable<Role>> GetRolesAsync(Guid id);
}
