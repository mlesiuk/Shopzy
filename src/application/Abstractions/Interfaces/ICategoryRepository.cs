using Shopzy.Domain.Entities;

namespace Shopzy.Application.Abstractions.Interfaces;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<Category?> FindByNameAsync(string name);
}
