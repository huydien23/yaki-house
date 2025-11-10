using Yakihouse.Domain.Common;
using Yakihouse.Domain.Enums;

namespace Yakihouse.Domain.Entities;

public class ShiftAttendance : EntityBase
{
    private ShiftAttendance()
    {
    }

    public ShiftAttendance(Guid shiftId, Guid staffId)
    {
        ShiftId = shiftId;
        StaffId = staffId;
        Status = ShiftAttendanceStatus.Scheduled;
    }

    public Guid ShiftId { get; private set; }
    public Shift Shift { get; private set; } = null!;
    public Guid StaffId { get; private set; }
    public Staff Staff { get; private set; } = null!;
    public DateTime? CheckIn { get; private set; }
    public DateTime? CheckOut { get; private set; }
    public ShiftAttendanceStatus Status { get; private set; }

    public void MarkCheckIn(DateTime timestamp)
    {
        CheckIn = timestamp;
        Status = ShiftAttendanceStatus.CheckedIn;
        Touch();
    }

    public void MarkCheckOut(DateTime timestamp)
    {
        CheckOut = timestamp;
        Status = ShiftAttendanceStatus.CheckedOut;
        Touch();
    }

    public void MarkMissed()
    {
        Status = ShiftAttendanceStatus.Missed;
        Touch();
    }
}

