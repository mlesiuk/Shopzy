using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopzy.Application.Commands.CategoryCommands;
using Shopzy.Application.Common;
using Shopzy.Application.Dtos;
using Shopzy.Application.Queries.CategoryQueries;

namespace Shopzy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ILogger<CategoryController> _logger;
    private readonly ISender _sender;

    public CategoryController(
        ILogger<CategoryController> logger,
        ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] GetCategoryByIdQuery getCategoryByIdQuery,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(getCategoryByIdQuery, cancellationToken);
        return result.Match<IActionResult>(
            success =>
            {
                return Ok(success.Adapt<CategoryDto>());
            },
            NotFound);
    }

    [Authorize(Policy = Consts.Administrator)]
    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody] CreateCategoryCommand createCategoryCommand,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(createCategoryCommand, cancellationToken);
        return result.Match<IActionResult>(
            Ok,
            validationException =>
            {
                _logger.LogError("There was an error during create process: {message}.", validationException.Message);
                return BadRequest(validationException.Errors);
            },
            Conflict);
    }

    [Authorize(Policy = Consts.Administrator)]
    [HttpPut]
    public async Task<IActionResult> Put(
        [FromBody] UpdateCategoryCommand updateCategoryCommand,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(updateCategoryCommand, cancellationToken);
        return result.Match<IActionResult>(
            Ok,
            validationException =>
            {
                _logger.LogError("There was an error during update process: {message}.", validationException.Message);
                return BadRequest(validationException.Errors);
            },
            Conflict);
    }

    [Authorize(Policy = Consts.Administrator)]
    [HttpPatch]
    public async Task<IActionResult> Patch(
        [FromBody] PatchCategoryCommand patchCategoryCommand,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(patchCategoryCommand, cancellationToken);
        return result.Match<IActionResult>(
            Ok,
            validationException =>
            {
                _logger.LogError("There was an error during update process: {message}.", validationException.Message);
                return BadRequest(validationException.Errors);
            },
            NotFound);
    }

    [Authorize(Policy = Consts.Administrator)]
    [HttpDelete]
    public async Task<IActionResult> Delete(
        [FromQuery] DeleteCategoryCommand deleteCategoryCommand,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(deleteCategoryCommand, cancellationToken);
        return result.Match<IActionResult>(
            deleted => NoContent(),
            notFound => NotFound());
    }
}
