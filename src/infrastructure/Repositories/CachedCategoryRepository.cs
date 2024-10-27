using Shopzy.Application.Abstractions.Interfaces;
using Shopzy.Domain.Entities;
using StackExchange.Redis;
using System.Text.Json;

namespace Shopzy.Infrastructure.Repositories;

public class CachedCategoryRepository : ICategoryRepository
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IDatabase _database;

    public CachedCategoryRepository(
        ICategoryRepository categoryRepository,
        IConnectionMultiplexer connectionMultiplexer)
    {
        _categoryRepository = categoryRepository;
        _connectionMultiplexer = connectionMultiplexer;
        _database = _connectionMultiplexer.GetDatabase();
    }

    public void Add(Category entity)
    {
        _categoryRepository.Add(entity);
        _database.StringSet(entity.Id.ToString(), JsonSerializer.Serialize(entity));
    }

    public void Delete(Category entity)
    {
        _categoryRepository.Delete(entity);
        _database.KeyDelete(entity.Id.ToString());
    }

    public async Task<Category?> FindByNameAsync(string name)
    {
        var result = await _categoryRepository.FindByNameAsync(name);
        if (result is not null)
        {
            await _database.StringSetAsync(result.Id.ToString(), JsonSerializer.Serialize(result));
        }
        return result;
    }

    public async Task<Category?> GetByIdAsync(Guid id, bool asNoTracking = true)
    {
        var cachedValue = await _database.StringGetAsync(id.ToString());
        if (!cachedValue.IsNullOrEmpty)
        {
            return JsonSerializer.Deserialize<Category>(cachedValue!);
        }
        
        var result = await _categoryRepository.GetByIdAsync(id, asNoTracking);
        if (result is not null)
        {
            await _database.StringSetAsync(id.ToString(), JsonSerializer.Serialize(result));
        }
        return result;
    }

    public void Update(Category entity)
    {
        _categoryRepository.Update(entity);
    }
}
