using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yakihouse.Application.Common.Interfaces;
using Yakihouse.Domain.Entities;
using Yakihouse.Infrastructure.Persistence;

namespace Yakihouse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StaffController : ControllerBase
{
    private readonly YakihouseDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public StaffController(YakihouseDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStaff([FromQuery] string? role, CancellationToken cancellationToken)
    {
        var query = _context.Staff
            .Include(s => s.Role)
            .AsQueryable();

        if (!string.IsNullOrEmpty(role))
        {
            query = query.Where(s => s.Role.Name == role);
        }

        var staff = await query
            .Select(s => new
            {
                s.Id,
                s.FullName,
                s.Email,
                s.Phone,
                RoleId = s.RoleId,
                RoleName = s.Role.Name,
                s.IsActive,
                s.HireDate,
                s.CreatedAt
            })
            .OrderBy(s => s.FullName)
            .ToListAsync(cancellationToken);

        return Ok(staff);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStaff(Guid id, CancellationToken cancellationToken)
    {
        var staff = await _context.Staff
            .Include(s => s.Role)
            .Where(s => s.Id == id)
            .Select(s => new
            {
                s.Id,
                s.FullName,
                s.Email,
                s.Phone,
                RoleId = s.RoleId,
                RoleName = s.Role.Name,
                s.IsActive,
                s.HireDate,
                s.CreatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (staff == null)
            return NotFound();

        return Ok(staff);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStaff([FromBody] CreateStaffRequest request, CancellationToken cancellationToken)
    {
        // Check if email already exists
        var emailExists = await _context.Staff.AnyAsync(s => s.Email == request.Email, cancellationToken);
        if (emailExists)
            return BadRequest(new { message = "Email đã tồn tại" });

        // Check if role exists
        var role = await _context.Roles.FindAsync(new object[] { request.RoleId }, cancellationToken);
        if (role == null)
            return BadRequest(new { message = "Vai trò không tồn tại" });

        // Create staff
        var staff = new Staff(
            request.FullName,
            request.Email,
            request.Phone,
            request.RoleId,
            request.HireDate
        );

        _context.Staff.Add(staff);

        // Create user account with default password
        var defaultPassword = "Yakihouse@123"; // Default password
        var hashedPassword = _passwordHasher.HashPassword(defaultPassword);
        
        var user = new User(
            request.Email, // Username = email
            hashedPassword,
            request.FullName,
            request.Email,
            role.Name
        );

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return Ok(new { 
            id = staff.Id, 
            fullName = staff.FullName, 
            email = staff.Email,
            defaultPassword = defaultPassword,
            message = "Nhân viên đã được tạo. Mật khẩu mặc định: " + defaultPassword
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStaff(Guid id, [FromBody] UpdateStaffRequest request, CancellationToken cancellationToken)
    {
        var staff = await _context.Staff.FindAsync(new object[] { id }, cancellationToken);
        if (staff == null)
            return NotFound();

        // Check if role exists
        if (request.RoleId != staff.RoleId)
        {
            var role = await _context.Roles.FindAsync(new object[] { request.RoleId }, cancellationToken);
            if (role == null)
                return BadRequest(new { message = "Vai trò không tồn tại" });

            // Update user role as well
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == staff.Email, cancellationToken);
            if (user != null)
            {
                user.UpdateRole(role.Name);
            }
        }

        staff.UpdateDetails(request.FullName, request.Email, request.Phone, request.RoleId);
        await _context.SaveChangesAsync(cancellationToken);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStaff(Guid id, CancellationToken cancellationToken)
    {
        var staff = await _context.Staff.FindAsync(new object[] { id }, cancellationToken);
        if (staff == null)
            return NotFound();

        // Also delete associated user account
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == staff.Email, cancellationToken);
        if (user != null)
        {
            _context.Users.Remove(user);
        }

        _context.Staff.Remove(staff);
        await _context.SaveChangesAsync(cancellationToken);

        return Ok();
    }

    [HttpPatch("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStaffStatus(Guid id, CancellationToken cancellationToken)
    {
        var staff = await _context.Staff.FindAsync(new object[] { id }, cancellationToken);
        if (staff == null)
            return NotFound();

        staff.SetActive(!staff.IsActive);
        await _context.SaveChangesAsync(cancellationToken);

        return Ok(new { isActive = staff.IsActive });
    }

    [HttpPost("{id}/reset-password")]
    public async Task<IActionResult> ResetPassword(Guid id, CancellationToken cancellationToken)
    {
        var staff = await _context.Staff.FindAsync(new object[] { id }, cancellationToken);
        if (staff == null)
            return NotFound();

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == staff.Email, cancellationToken);
        if (user == null)
            return NotFound(new { message = "Không tìm thấy tài khoản người dùng" });

        var newPassword = "Yakihouse@123"; // Default password
        var hashedPassword = _passwordHasher.HashPassword(newPassword);
        user.UpdatePassword(hashedPassword);
        
        await _context.SaveChangesAsync(cancellationToken);

        return Ok(new { 
            message = "Mật khẩu đã được đặt lại",
            newPassword = newPassword
        });
    }

    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
    {
        var roles = await _context.Roles
            .Select(r => new
            {
                r.Id,
                r.Name,
                r.Description
            })
            .ToListAsync(cancellationToken);

        return Ok(roles);
    }
}

// DTOs
public record CreateStaffRequest(string FullName, string Email, string Phone, Guid RoleId, DateTime HireDate);
public record UpdateStaffRequest(string FullName, string Email, string Phone, Guid RoleId);
