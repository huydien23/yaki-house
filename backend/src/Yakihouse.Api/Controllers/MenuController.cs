using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yakihouse.Infrastructure.Persistence;

namespace Yakihouse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly YakihouseDbContext _context;

    public MenuController(YakihouseDbContext context)
    {
        _context = context;
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var categories = await _context.MenuCategories
            .Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.DisplayOrder
            })
            .ToListAsync(cancellationToken);

        return Ok(categories);
    }

    [HttpGet("items")]
    public async Task<IActionResult> GetMenuItems([FromQuery] Guid? categoryId, CancellationToken cancellationToken)
    {
        var query = _context.MenuItems
            .Include(i => i.Category)
            .Include(i => i.OptionGroups)
                .ThenInclude(g => g.Options)
            .Where(i => i.Status == Domain.Enums.MenuItemStatus.Available);

        if (categoryId.HasValue)
        {
            query = query.Where(i => i.CategoryId == categoryId.Value);
        }

        var items = await query
            .Select(i => new
            {
                i.Id,
                i.Name,
                i.Description,
                i.BasePrice,
                CategoryId = i.CategoryId,
                CategoryName = i.Category.Name,
                OptionGroups = i.OptionGroups.Select(g => new
                {
                    g.Id,
                    g.Name,
                    g.IsRequired,
                    Options = g.Options.Select(o => new
                    {
                        o.Id,
                        o.Name,
                        o.ExtraPrice,
                        o.IsDefault
                    }).ToList()
                }).ToList()
            })
            .ToListAsync(cancellationToken);

        return Ok(items);
    }

    [HttpGet("items/{id}")]
    public async Task<IActionResult> GetMenuItem(Guid id, CancellationToken cancellationToken)
    {
        var item = await _context.MenuItems
            .Include(i => i.Category)
            .Include(i => i.OptionGroups)
                .ThenInclude(g => g.Options)
            .Where(i => i.Id == id)
            .Select(i => new
            {
                i.Id,
                i.Name,
                i.Description,
                i.BasePrice,
                CategoryId = i.CategoryId,
                CategoryName = i.Category.Name,
                OptionGroups = i.OptionGroups.Select(g => new
                {
                    g.Id,
                    g.Name,
                    g.IsRequired,
                    Options = g.Options.Select(o => new
                    {
                        o.Id,
                        o.Name,
                        o.ExtraPrice,
                        o.IsDefault
                    }).ToList()
                }).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (item == null)
            return NotFound();

        return Ok(item);
    }
}

