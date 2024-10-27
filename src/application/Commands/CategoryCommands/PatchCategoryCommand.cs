using FluentValidation;
using MediatR;
using OneOf;
using Shopzy.Application.Abstractions.Interfaces;
using Shopzy.Application.Exceptions;
using Shopzy.Domain.Entities;

namespace Shopzy.Application.Commands.CategoryCommands;

public sealed class PatchCategoryCommand : IRequest<OneOf<Category, ValidationException, NotFoundException>>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public sealed class PatchCategoryCommandHandler : IRequestHandler<PatchCategoryCommand, OneOf<Category, ValidationException, NotFoundException>>
{
    private readonly ICategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<PatchCategoryCommand> _validator;

    public PatchCategoryCommandHandler(
        ICategoryRepository repository,
        IUnitOfWork unitOfWork,
        IValidator<PatchCategoryCommand> validator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<OneOf<Category, ValidationException, NotFoundException>> Handle(PatchCategoryCommand request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        var failures = validationResult.Errors?.ToList();
        if (failures is not null && failures!.Any())
        {
            return new ValidationException(failures);
        }

        var category = await _repository.GetByIdAsync(request.Id);
        if (category == null)
        {
            return new NotFoundException(nameof(request), request.Id);
        }

        var newCategory = Category.Create(request.Name);
        _repository.Update(newCategory);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return newCategory;
    }
}
