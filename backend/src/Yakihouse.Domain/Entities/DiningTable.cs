using Yakihouse.Domain.Common;
using Yakihouse.Domain.Enums;

namespace Yakihouse.Domain.Entities;

public class DiningTable : AggregateRoot
{
    private readonly List<TableAssignment> _assignments = new();

    private DiningTable()
    {
        // EF Core
    }

    public DiningTable(string code, string? zone, int capacity)
    {
        Code = code;
        Zone = zone;
        Capacity = capacity;
        Status = TableStatus.Available;
    }

    public string Code { get; private set; } = null!;
    public string? Zone { get; private set; }
    public int Capacity { get; private set; }
    public TableStatus Status { get; private set; }
    public IReadOnlyCollection<TableAssignment> Assignments => _assignments.AsReadOnly();

    public void UpdateInfo(string code, string? zone, int capacity)
    {
        Code = code;
        Zone = zone;
        Capacity = capacity;
        Touch();
    }

    public void SetStatus(TableStatus status)
    {
        Status = status;
        Touch();
    }

    public TableAssignment AssignToStaff(Guid staffId, Guid shiftId)
    {
        var assignment = new TableAssignment(Id, staffId, shiftId);
        _assignments.Add(assignment);
        Touch();
        return assignment;
    }
}

