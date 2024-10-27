using MediatR;
using Shopzy.Application.Abstractions.Interfaces;
using Shopzy.Application.Commands.UserCommands;

namespace Shopzy.Application.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ICurrentUserService _currentUserService;

    public AuthorizationBehaviour(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        var requestType = request.GetType();
        if (requestType.Equals(typeof(LoginUserCommand)) || requestType.Equals(typeof(CreateUserCommand)))
        {
            return await next();
        }
        
        if (string.IsNullOrEmpty(_currentUserService.UserName))
        {
            throw new UnauthorizedAccessException();
        }
        return await next();
    }
}
