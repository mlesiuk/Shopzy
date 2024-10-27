using MediatR;
using OneOf;
using Shopzy.Application.Abstractions.Interfaces;
using Shopzy.Application.Exceptions;
using Shopzy.Domain.Entities;

namespace Shopzy.Application.Commands.CategoryCommands;

public sealed class DeleteCategoryCommand : IRequest<OneOf<Task, NotFoundException>>
{
    public Guid Id { get; set; }
}

public sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, OneOf<Task, NotFoundException>>
{
    private readonly ICategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryCommandHandler(
        ICategoryRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<Task, NotFoundException>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken = default)
    {
        var id = request.Id;
        var category = await _repository.GetByIdAsync(id);
        if (category == null)
        {
            return new NotFoundException(nameof(Category), id);
        }

        _repository.Delete(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Task.CompletedTask;
    }
}
