namespace Yakihouse.Domain.Enums;

public enum OrderStatus
{
    Draft = 0,
    Submitted = 1,
    InProgress = 2, // Đang chế biến
    Ready = 3, // Sẵn sàng
    Locked = 4,
    Completed = 5,
    Cancelled = 6
}

