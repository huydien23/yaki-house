using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yakihouse.Infrastructure.Persistence;

namespace Yakihouse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TablesController : ControllerBase
{
    private readonly YakihouseDbContext _context;

    public TablesController(YakihouseDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetTables([FromQuery] string? zone, CancellationToken cancellationToken)
    {
        var query = _context.DiningTables.AsQueryable();

        if (!string.IsNullOrEmpty(zone))
        {
            query = query.Where(t => t.Zone == zone);
        }

        var tables = await query
            .OrderBy(t => t.Code)
            .Select(t => new
            {
                t.Id,
                t.Code,
                t.Zone,
                t.Capacity,
                Status = t.Status.ToString()
            })
            .ToListAsync(cancellationToken);

        return Ok(tables);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTable(Guid id, CancellationToken cancellationToken)
    {
        var table = await _context.DiningTables
            .Where(t => t.Id == id)
            .Select(t => new
            {
                t.Id,
                t.Code,
                t.Zone,
                t.Capacity,
                Status = t.Status.ToString()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (table == null)
            return NotFound();

        return Ok(table);
    }

    [HttpGet("zones")]
    public async Task<IActionResult> GetZones(CancellationToken cancellationToken)
    {
        var zones = await _context.DiningTables
            .Where(t => !string.IsNullOrEmpty(t.Zone))
            .Select(t => t.Zone)
            .Distinct()
            .ToListAsync(cancellationToken);

        return Ok(zones);
    }
}

