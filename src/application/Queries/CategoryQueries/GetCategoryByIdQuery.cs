using MediatR;
using OneOf;
using Shopzy.Application.Abstractions.Interfaces;
using Shopzy.Application.Exceptions;
using Shopzy.Domain.Entities;

namespace Shopzy.Application.Queries;

public sealed class GetCategoryByIdQuery 
    : IRequest<OneOf<Category, NotFoundException>>
{
    public Guid Id { get; set; }
}

public sealed class GetCategoryByIdQueryHandler 
    : IRequestHandler<GetCategoryByIdQuery, OneOf<Category, NotFoundException>>
{
    private readonly ICategoryRepository _repository;

    public GetCategoryByIdQueryHandler(
        ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<OneOf<Category, NotFoundException>> Handle(
        GetCategoryByIdQuery request, 
        CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id);
        if (category is null)
        {
            return new NotFoundException(nameof(Category), request.Id);
        }
        return category;
    }
}
