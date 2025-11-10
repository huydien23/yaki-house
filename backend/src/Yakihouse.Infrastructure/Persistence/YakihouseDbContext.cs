using Microsoft.EntityFrameworkCore;
using Yakihouse.Domain.Entities;

namespace Yakihouse.Infrastructure.Persistence;

public class YakihouseDbContext : DbContext
{
    public YakihouseDbContext(DbContextOptions<YakihouseDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<Shift> Shifts => Set<Shift>();
    public DbSet<ShiftAttendance> ShiftAttendances => Set<ShiftAttendance>();
    public DbSet<DiningTable> DiningTables => Set<DiningTable>();
    public DbSet<TableAssignment> TableAssignments => Set<TableAssignment>();
    public DbSet<MenuCategory> MenuCategories => Set<MenuCategory>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<MenuOptionGroup> MenuOptionGroups => Set<MenuOptionGroup>();
    public DbSet<MenuOption> MenuOptions => Set<MenuOption>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<OrderItemOption> OrderItemOptions => Set<OrderItemOption>();
    public DbSet<OrderAudit> OrderAudits => Set<OrderAudit>();
    public DbSet<KitchenStation> KitchenStations => Set<KitchenStation>();
    public DbSet<KitchenTicket> KitchenTickets => Set<KitchenTicket>();
    public DbSet<KitchenTicketItem> KitchenTicketItems => Set<KitchenTicketItem>();
    public DbSet<Bill> Bills => Set<Bill>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<BillPromotion> BillPromotions => Set<BillPromotion>();
    public DbSet<Promotion> Promotions => Set<Promotion>();
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<InventoryTransaction> InventoryTransactions => Set<InventoryTransaction>();
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<PayrollSetting> PayrollSettings => Set<PayrollSetting>();
    public DbSet<PayrollEntry> PayrollEntries => Set<PayrollEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(YakihouseDbContext).Assembly);
    }
}

