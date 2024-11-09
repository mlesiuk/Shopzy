using Shopzy.Domain.Entities;

namespace Shopzy.Application.Abstractions.Interfaces;

public interface IUserRoleRepository : IBaseRepository<UserRole>
{
    Task<IEnumerable<Role>> AssignRoleAsync(User user, Role role);
}
