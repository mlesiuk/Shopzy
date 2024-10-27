using FluentValidation;
using Mapster;
using MediatR;
using OneOf;
using Shopzy.Application.Abstractions.Interfaces;
using Shopzy.Application.Dtos;
using Shopzy.Application.Exceptions;
using Shopzy.Domain.Entities;

namespace Shopzy.Application.Commands.CategoryCommands;

public sealed class CreateCategoryCommand 
    : IRequest<OneOf<CategoryDto, ValidationException, AlreadyExistsException>>
{
    public string Name { get; init; } = string.Empty;
}

public sealed class CreateCategoryCommandHandler 
    : IRequestHandler<CreateCategoryCommand, OneOf<CategoryDto, ValidationException, AlreadyExistsException>>
{
    private readonly ICategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateCategoryCommand> _validator;

    public CreateCategoryCommandHandler(
        ICategoryRepository repository,
        IUnitOfWork unitOfWork,
        IValidator<CreateCategoryCommand> validator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<OneOf<CategoryDto, ValidationException, AlreadyExistsException>> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        var failures = validationResult.Errors?.ToList();
        if (failures is not null && failures!.Any())
        {
            return new ValidationException(failures);
        }

        var categoryDto = await _repository.FindByNameAsync(request.Name);
        if (categoryDto != null)
        {
            return new AlreadyExistsException(categoryDto);
        }

        var category = Category.Create(request.Name);
        _repository.Add(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return category.Adapt<CategoryDto>();
    }
}
