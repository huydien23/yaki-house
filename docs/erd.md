## ERD Tổng Quan

```
Staff (StaffId, FullName, RoleId, Phone, Email, Status)
Role (RoleId, Name, Description)
Shift (ShiftId, Name, StartTime, EndTime, ShiftType)
ShiftAttendance (AttendanceId, ShiftId, StaffId, CheckIn, CheckOut, Status)

DiningTable (TableId, Code, Zone, Capacity, Status)
TableAssignment (AssignmentId, TableId, StaffId, ShiftId, AssignedAt, ReleasedAt)

MenuCategory (CategoryId, Name, DisplayOrder, IsActive)
MenuItem (ItemId, CategoryId, Name, Description, BasePrice, Status)
MenuOptionGroup (GroupId, ItemId, Name, IsRequired)
MenuOption (OptionId, GroupId, Name, ExtraPrice, IsDefault)

Order (OrderId, TableId, StaffId, Status, GuestCount, CreatedAt, ClosedAt, Notes)
OrderItem (OrderItemId, OrderId, MenuItemId, Quantity, UnitPrice, Status, Notes)
OrderItemOption (OrderItemOptionId, OrderItemId, MenuOptionId, Quantity)
OrderAudit (OrderAuditId, OrderId, ActionType, ActorId, Metadata, CreatedAt)

KitchenTicket (TicketId, OrderId, StationId, Status, StartedAt, CompletedAt)
KitchenTicketItem (TicketItemId, TicketId, OrderItemId, Status, Notes)
KitchenStation (StationId, Name, DisplayOrder, IsActive)

Bill (BillId, OrderId, SubTotal, DiscountTotal, ServiceCharge, Tax, GrandTotal, Status)
Payment (PaymentId, BillId, Method, Amount, ReferenceCode, PaidAt, Status)
Promotion (PromotionId, Name, Type, Value, StartDate, EndDate, Conditions)
BillPromotion (BillPromotionId, BillId, PromotionId, AppliedValue)

InventoryItem (InventoryItemId, Name, Unit, SafetyStock, CurrentStock)
Recipe (RecipeId, MenuItemId, InventoryItemId, QuantityPerServing)
InventoryTransaction (TransactionId, InventoryItemId, Type, Quantity, ReferenceId, CreatedAt)

PayrollSetting (SettingId, RoleId, BaseRate, Allowance)
PayrollEntry (PayrollEntryId, StaffId, ShiftId, Period, GrossPay, Adjustments, NetPay, Status)
```

## Quan Hệ Chính

- `Staff` (1..*) — (1) `Role`: mỗi nhân viên gắn một vai trò chính.
- `ShiftAttendance` liên kết nhân viên và ca làm, ghi nhận chấm công.
- `DiningTable` — `TableAssignment`: lưu lịch sử bàn do ai quản lý trong ca.
- `Order` tham chiếu `DiningTable` & `Staff`; `OrderItem` thuộc về `Order`.
- `OrderItemOption` ánh xạ các lựa chọn thêm của món.
- `KitchenTicket` được sinh từ order, nhóm theo `KitchenStation` để chia line bếp.
- `Bill` gắn với `Order`; `Payment` gắn với `Bill`.
- `Promotion` áp dụng qua bảng trung gian `BillPromotion`.
- `Recipe` nối `MenuItem` với `InventoryItem` để phục vụ module tồn kho.
- `PayrollEntry` sử dụng dữ liệu `ShiftAttendance` + `Bill`/`Order` để tính KPI.

## Ghi Chú Thiết Kế

- Sử dụng `Guid` cho các khóa chính nhằm hỗ trợ đồng bộ offline.
- Trường `Status` chuẩn hóa theo enum: ví dụ `OrderStatus` (Draft, Submitted, Locked, Completed, Cancelled).
- `OrderAudit.Metadata` lưu JSON (sử dụng `jsonb` nếu SQL Server 2022+ hoặc `nvarchar(max)` + `OPENJSON`).
- `KitchenTicket` cho phép split một order thành nhiều ticket theo trạm bếp.
- `Inventory` và `Payroll` có thể bật/tắt qua feature flag để triển khai theo lộ trình.

