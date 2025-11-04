using Yakihouse.Domain.Common;

namespace Yakihouse.Domain.Entities;

public class TableAssignment : EntityBase
{
    private TableAssignment()
    {
    }

    public TableAssignment(Guid tableId, Guid staffId, Guid shiftId)
    {
        TableId = tableId;
        StaffId = staffId;
        ShiftId = shiftId;
        AssignedAt = DateTime.UtcNow;
    }

    public Guid TableId { get; private set; }
    public DiningTable Table { get; private set; } = null!;
    public Guid StaffId { get; private set; }
    public Staff Staff { get; private set; } = null!;
    public Guid ShiftId { get; private set; }
    public Shift Shift { get; private set; } = null!;
    public DateTime AssignedAt { get; private set; }
    public DateTime? ReleasedAt { get; private set; }

    public void Release()
    {
        if (ReleasedAt is not null)
        {
            return;
        }

        ReleasedAt = DateTime.UtcNow;
        Touch();
    }
}

