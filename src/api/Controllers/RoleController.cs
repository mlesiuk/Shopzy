using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopzy.Application.Common;
using Shopzy.Application.Commands.RoleCommands;

namespace Shopzy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly ILogger<RoleController> _logger;
    private readonly ISender _sender;

    public RoleController(
        ILogger<RoleController> logger,
        ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    [Authorize(Policy = Consts.Administrator)]
    [HttpPost(nameof(Create))]
    public async Task<IActionResult> Create(
        [FromBody] CreateRoleCommand createRoleCommand,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(createRoleCommand, cancellationToken);
        return result.Match<IActionResult>(
            Ok,
            BadRequest,
            Conflict);
    }

    [Authorize(Policy = Consts.Administrator)]
    [HttpPost(nameof(Assign))]
    public async Task<IActionResult> Assign(
        [FromBody] AssignRoleCommand assignRoleCommand,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(assignRoleCommand, cancellationToken);
        return result.Match<IActionResult>(
            Ok,
            BadRequest,
            Conflict);
    }
}
