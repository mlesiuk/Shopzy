using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopzy.Application.Commands.UserCommands;

namespace Shopzy.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly ISender _sender;

    public LoginController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserCommand loginUserCommand,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(loginUserCommand, cancellationToken);
        return result.Match<IActionResult>(
            Ok,
            BadRequest,
            Conflict);
    }
}
