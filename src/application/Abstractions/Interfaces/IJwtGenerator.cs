using Shopzy.Application.Dtos;
using Shopzy.Domain.Entities;

namespace Shopzy.Application.Abstractions.Interfaces;

public interface IJwtGenerator
{
    JwtDto Generate(User User, IEnumerable<Role> userRoles);
}
