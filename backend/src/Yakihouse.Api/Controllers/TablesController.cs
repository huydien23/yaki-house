using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yakihouse.Domain.Entities;
using Yakihouse.Domain.Enums;
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

    [HttpPost]
    public async Task<IActionResult> CreateTable([FromBody] CreateTableRequest request, CancellationToken cancellationToken)
    {
        // Check if code already exists
        var codeExists = await _context.DiningTables.AnyAsync(t => t.Code == request.Code, cancellationToken);
        if (codeExists)
            return BadRequest(new { message = "Mã bàn đã tồn tại" });

        var table = new DiningTable(request.Code, request.Capacity, request.Zone);
        _context.DiningTables.Add(table);
        await _context.SaveChangesAsync(cancellationToken);

        return Ok(new { 
            id = table.Id, 
            code = table.Code, 
            zone = table.Zone,
            capacity = table.Capacity,
            status = table.Status.ToString()
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTable(Guid id, [FromBody] UpdateTableRequest request, CancellationToken cancellationToken)
    {
        var table = await _context.DiningTables.FindAsync(new object[] { id }, cancellationToken);
        if (table == null)
            return NotFound();

        // Check if new code already exists (excluding current table)
        if (request.Code != table.Code)
        {
            var codeExists = await _context.DiningTables
                .AnyAsync(t => t.Code == request.Code && t.Id != id, cancellationToken);
            if (codeExists)
                return BadRequest(new { message = "Mã bàn đã tồn tại" });
        }

        table.UpdateDetails(request.Code, request.Capacity, request.Zone);
        await _context.SaveChangesAsync(cancellationToken);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTable(Guid id, CancellationToken cancellationToken)
    {
        var table = await _context.DiningTables.FindAsync(new object[] { id }, cancellationToken);
        if (table == null)
            return NotFound();

        // Check if table is currently occupied
        if (table.Status == TableStatus.Seated)
            return BadRequest(new { message = "Không thể xóa bàn đang có khách" });

        _context.DiningTables.Remove(table);
        await _context.SaveChangesAsync(cancellationToken);

        return Ok();
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateTableStatus(Guid id, [FromBody] UpdateTableStatusRequest request, CancellationToken cancellationToken)
    {
        var table = await _context.DiningTables.FindAsync(new object[] { id }, cancellationToken);
        if (table == null)
            return NotFound();

        table.SetStatus(request.Status);
        await _context.SaveChangesAsync(cancellationToken);

        return Ok(new { status = table.Status.ToString() });
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

// DTOs
public record CreateTableRequest(string Code, int Capacity, string? Zone);
public record UpdateTableRequest(string Code, int Capacity, string? Zone);
public record UpdateTableStatusRequest(TableStatus Status);


