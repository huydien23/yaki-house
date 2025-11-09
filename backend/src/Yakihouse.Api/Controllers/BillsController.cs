using MediatR;
using Microsoft.AspNetCore.Mvc;
using Yakihouse.Application.Billing.Commands.CreateBill;

namespace Yakihouse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BillsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BillsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBill([FromBody] CreateBillCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetBill), new { id = result.Data!.Id }, result.Data);
    }

    [HttpGet("{id}")]
    public Task<IActionResult> GetBill(Guid id, CancellationToken cancellationToken)
    {
        // TODO: Implement GetBill query
        return Task.FromResult<IActionResult>(Ok());
    }
}

