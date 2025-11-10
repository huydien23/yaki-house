using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yakihouse.Domain.Entities;
using Yakihouse.Domain.Enums;
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
            .OrderBy(c => c.DisplayOrder)
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.DisplayOrder,
                c.IsActive
            })
            .ToListAsync(cancellationToken);

        return Ok(categories);
    }

    [HttpPost("categories")]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = new MenuCategory(request.Name, request.DisplayOrder);
        _context.MenuCategories.Add(category);
        await _context.SaveChangesAsync(cancellationToken);

        return Ok(new { id = category.Id, name = category.Name, displayOrder = category.DisplayOrder });
    }

    [HttpPut("categories/{id}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await _context.MenuCategories.FindAsync(new object[] { id }, cancellationToken);
        if (category == null)
            return NotFound();

        category.UpdateDetails(request.Name, request.DisplayOrder);
        await _context.SaveChangesAsync(cancellationToken);

        return Ok();
    }

    [HttpDelete("categories/{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken cancellationToken)
    {
        var category = await _context.MenuCategories
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        
        if (category == null)
            return NotFound();

        if (category.Items.Any())
            return BadRequest(new { message = "Không thể xóa danh mục có món ăn. Hãy xóa hoặc chuyển các món ăn sang danh mục khác trước." });

        _context.MenuCategories.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);

        return Ok();
    }

    [HttpGet("items")]
    public async Task<IActionResult> GetMenuItems([FromQuery] Guid? categoryId, CancellationToken cancellationToken)
    {
        var query = _context.MenuItems
            .Include(i => i.Category)
            .Include(i => i.OptionGroups)
                .ThenInclude(g => g.Options)
            .AsQueryable();

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
                Status = i.Status.ToString(),
                IsAvailable = i.Status == MenuItemStatus.Available,
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

    [HttpPost("items")]
    public async Task<IActionResult> CreateMenuItem([FromBody] CreateMenuItemRequest request, CancellationToken cancellationToken)
    {
        var category = await _context.MenuCategories.FindAsync(new object[] { request.CategoryId }, cancellationToken);
        if (category == null)
            return BadRequest(new { message = "Danh mục không tồn tại" });

        var item = category.AddItem(request.Name, request.Description, request.BasePrice);
        if (request.KitchenStationId.HasValue)
        {
            // Set kitchen station if provided
            var stationProperty = typeof(MenuItem).GetProperty("KitchenStationId");
            stationProperty?.SetValue(item, request.KitchenStationId);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Ok(new { 
            id = item.Id, 
            name = item.Name, 
            basePrice = item.BasePrice,
            categoryId = item.CategoryId
        });
    }

    [HttpPut("items/{id}")]
    public async Task<IActionResult> UpdateMenuItem(Guid id, [FromBody] UpdateMenuItemRequest request, CancellationToken cancellationToken)
    {
        var item = await _context.MenuItems.FindAsync(new object[] { id }, cancellationToken);
        if (item == null)
            return NotFound();

        item.UpdateDetails(request.Name, request.Description, request.BasePrice);
        
        // Update category if changed
        if (request.CategoryId != item.CategoryId)
        {
            var categoryProperty = typeof(MenuItem).GetProperty("CategoryId");
            categoryProperty?.SetValue(item, request.CategoryId);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Ok();
    }

    [HttpDelete("items/{id}")]
    public async Task<IActionResult> DeleteMenuItem(Guid id, CancellationToken cancellationToken)
    {
        var item = await _context.MenuItems.FindAsync(new object[] { id }, cancellationToken);
        if (item == null)
            return NotFound();

        _context.MenuItems.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);

        return Ok();
    }

    [HttpPatch("items/{id}/status")]
    public async Task<IActionResult> UpdateItemStatus(Guid id, [FromBody] UpdateItemStatusRequest request, CancellationToken cancellationToken)
    {
        var item = await _context.MenuItems.FindAsync(new object[] { id }, cancellationToken);
        if (item == null)
            return NotFound();

        var status = request.IsAvailable ? MenuItemStatus.Available : MenuItemStatus.OutOfStock;
        item.SetStatus(status);
        await _context.SaveChangesAsync(cancellationToken);

        return Ok();
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

// DTOs
public record CreateCategoryRequest(string Name, int DisplayOrder);
public record UpdateCategoryRequest(string Name, int DisplayOrder);
public record CreateMenuItemRequest(string Name, string? Description, decimal BasePrice, Guid CategoryId, Guid? KitchenStationId);
public record UpdateMenuItemRequest(string Name, string? Description, decimal BasePrice, Guid CategoryId);
public record UpdateItemStatusRequest(bool IsAvailable);


