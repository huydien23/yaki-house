using MediatR;
using Microsoft.AspNetCore.Mvc;
using Yakihouse.Application.Common.Models;

namespace Yakihouse.Api.Controllers;

/// <summary>
/// Base controller with common functionality for all API controllers
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public abstract class BaseApiController : ControllerBase
{
    protected IMediator Mediator => HttpContext.RequestServices.GetRequiredService<IMediator>();
    protected ILogger Logger => HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
        .CreateLogger(GetType());

    /// <summary>
    /// Returns OK result from successful operation
    /// </summary>
    protected IActionResult OkResult<T>(Result<T> result)
    {
        if (!result.IsSuccess)
        {
            return HandleFailure(result);
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns Created result from successful creation
    /// </summary>
    protected IActionResult CreatedResult<T>(Result<T> result, string actionName, object routeValues)
    {
        if (!result.IsSuccess)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(actionName, routeValues, result.Data);
    }

    /// <summary>
    /// Returns NoContent result from successful operation without data
    /// </summary>
    protected IActionResult NoContentResult(Result result)
    {
        if (!result.IsSuccess)
        {
            return HandleFailure(result);
        }

        return NoContent();
    }

    /// <summary>
    /// Handles failed results appropriately
    /// </summary>
    private IActionResult HandleFailure<T>(Result<T> result)
    {
        if (result.Errors.Any())
        {
            return BadRequest(new
            {
                errors = result.Errors
            });
        }

        return BadRequest(new
        {
            error = result.Error
        });
    }

    private IActionResult HandleFailure(Result result)
    {
        if (result.Errors.Any())
        {
            return BadRequest(new
            {
                errors = result.Errors
            });
        }

        return BadRequest(new
        {
            error = result.Error
        });
    }
}
