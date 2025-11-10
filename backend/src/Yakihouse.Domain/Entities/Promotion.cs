using Yakihouse.Domain.Common;

namespace Yakihouse.Domain.Entities;

public class Promotion : AggregateRoot
{
    private Promotion()
    {
    }

    public Promotion(string name, string type, decimal value, DateTime startDate, DateTime endDate, string? conditions)
    {
        Name = name;
        Type = type;
        Value = value;
        StartDate = startDate;
        EndDate = endDate;
        Conditions = conditions;
        IsActive = true;
    }

    public string Name { get; private set; } = null!;
    public string Type { get; private set; } = null!;
    public decimal Value { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public string? Conditions { get; private set; }
    public bool IsActive { get; private set; }

    public void UpdateDetails(string name, string type, decimal value, DateTime startDate, DateTime endDate, string? conditions)
    {
        Name = name;
        Type = type;
        Value = value;
        StartDate = startDate;
        EndDate = endDate;
        Conditions = conditions;
        Touch();
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
        Touch();
    }
}

