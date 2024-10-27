using Shopzy.Domain.Entities;

namespace Shopzy.Infrastructure.Data;

public class Seed
{
    public IEnumerable<User> Users { get; set; } = Enumerable.Empty<User>();
    public IEnumerable<Role> Roles { get; set; } = Enumerable.Empty<Role>();
}
