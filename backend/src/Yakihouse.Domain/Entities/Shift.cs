using Yakihouse.Domain.Common;
using Yakihouse.Domain.Enums;

namespace Yakihouse.Domain.Entities;

public class Shift : AggregateRoot
{
    private Shift()
    {
    }

    public Shift(string name, TimeSpan startTime, TimeSpan endTime, string type)
    {
        Name = name;
        StartTime = startTime;
        EndTime = endTime;
        ShiftType = type;
        Status = ShiftStatus.Draft;
    }

    public string Name { get; private set; } = null!;
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }
    public string ShiftType { get; private set; } = null!;
    public ShiftStatus Status { get; private set; }

    public void UpdateDetails(string name, TimeSpan startTime, TimeSpan endTime, string type)
    {
        Name = name;
        StartTime = startTime;
        EndTime = endTime;
        ShiftType = type;
        Touch();
    }

    public void SetStatus(ShiftStatus status)
    {
        Status = status;
        Touch();
    }
}

