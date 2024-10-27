using Shopzy.Domain.Entities;

namespace Shopzy.Application.Abstractions.Interfaces;

public interface IRoleRepository : IBaseRepository<Role>
{
    Task<Role?> FindByNameAsync(string name);
}
