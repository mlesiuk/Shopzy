using FluentValidation;
using MediatR;
using OneOf;
using Shopzy.Application.Abstractions.Interfaces;
using Shopzy.Application.Exceptions;
using Shopzy.Domain.Entities;

namespace Shopzy.Application.Commands.CategoryCommands;

public sealed class UpdateCategoryCommand : IRequest<OneOf<Category, ValidationException, AlreadyExistsException>>
{
    public string Name { get; set; } = string.Empty;
}

public sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, OneOf<Category, ValidationException, AlreadyExistsException>>
{
    private readonly ICategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateCategoryCommand> _validator;

    public UpdateCategoryCommandHandler(
        ICategoryRepository repository,
        IUnitOfWork unitOfWork,
        IValidator<UpdateCategoryCommand> validator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<OneOf<Category, ValidationException, AlreadyExistsException>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        var failures = validationResult.Errors?.ToList();
        if (failures is not null && failures!.Any())
        {
            return new ValidationException(failures);
        }

        Category category = Category.Create(request.Name);
        var catgoryAlreadyExists = await _repository.FindByNameAsync(request.Name);
        if (catgoryAlreadyExists != null)
        {
            _repository.Update(category);
        }
        else
        {
            _repository.Add(category);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return category;
    }
}
