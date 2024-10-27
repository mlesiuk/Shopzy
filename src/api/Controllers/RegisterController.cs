using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopzy.Application.Commands.UserCommands;

namespace Shopzy.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RegisterController : ControllerBase
{
    private readonly ILogger<RegisterController> _logger;
    private readonly ISender _sender;

    public RegisterController(
        ILogger<RegisterController> logger,
        ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    [HttpPost(nameof(Create))]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserCommand createUserCommand,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(createUserCommand, cancellationToken);
        return result.Match<IActionResult>(
            success => { return Created(nameof(Create), success); },
            BadRequest,
            Conflict);
    }
}
