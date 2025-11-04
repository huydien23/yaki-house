## Vai Trò Chính

| Role                | Mô tả nhiệm vụ                                                          |
|---------------------|-------------------------------------------------------------------------|
| Admin hệ thống      | Cấu hình tổng thể, phân quyền, quản lý người dùng, thiết lập thông số. |
| Quản lý ca          | Giám sát vận hành, duyệt hủy món, phân ca, xem báo cáo theo ca.         |
| Nhân viên phục vụ   | Quản lý order bàn phụ trách, cập nhật trạng thái, yêu cầu thanh toán.  |
| Nhân viên lễ tân    | Check-in khách, gán bàn, hỗ trợ thu ngân.                              |
| Thu ngân            | Xử lý thanh toán, in hóa đơn, hoàn tiền.                               |
| Bếp trưởng/KDS Lead | Phân bổ món, giám sát tiến độ, xử lý sự cố nguyên liệu.                |
| Tổ bếp              | Tiếp nhận món theo line, cập nhật trạng thái thực hiện.                |

## Ma Trận Quyền (Rút gọn)

| Module / Hành động                    | Admin | Quản lý | Phục vụ | Lễ tân | Thu ngân | Bếp trưởng | Tổ bếp |
|---------------------------------------|:-----:|:-------:|:-------:|:------:|:--------:|:----------:|:------:|
| Quản trị người dùng, phân quyền       |  ✔    |   ✖     |   ✖     |   ✖    |    ✖     |     ✖      |   ✖    |
| Cấu hình menu, giá, khuyến mãi        |  ✔    |   ✔     |   ✖     |   ✖    |    ✖     |     ✖      |   ✖    |
| Quản lý bàn & sơ đồ                   |  ✔    |   ✔     |   ✖     |   ✖    |    ✖     |     ✖      |   ✖    |
| Tạo/Chỉnh sửa order                   |  ✔    |   ✔     |   ✔     |   ✖    |    ✖     |     ✖      |   ✖    |
| Gửi yêu cầu hủy order sau khi locked  |  ✔    |   ✔     |   ⚠     |   ✖    |    ✖     |     ✔      |   ✖    |
| Duyệt yêu cầu hủy order               |  ✔    |   ✔     |   ✖     |   ✖    |    ✖     |     ✔      |   ✖    |
| Xử lý thanh toán                      |  ✔    |   ✔     |   ✖     |   ✖    |    ✔     |     ✖      |   ✖    |
| In hóa đơn / hoàn tiền                |  ✔    |   ✔     |   ✖     |   ✖    |    ✔     |     ✖      |   ✖    |
| Tiếp nhận món (KDS)                   |  ✔    |   ✔     |   ✖     |   ✖    |    ✖     |     ✔      |   ✔    |
| Cập nhật trạng thái món               |  ✔    |   ✔     |   ✖     |   ✖    |    ✖     |     ✔      |   ✔    |
| Báo cáo doanh thu, KPI                |  ✔    |   ✔     |   ⚠     |   ⚠    |    ✔     |     ⚠      |   ⚠    |
| Audit log & cấu hình nâng cao         |  ✔    |   ✖     |   ✖     |   ✖    |    ✖     |     ✖      |   ✖    |

Ghi chú:

- `✔`: quyền đầy đủ.
- `⚠`: quyền xem hạn chế (ví dụ chỉ xem KPI cá nhân).
- `✖`: không có quyền.

## Quy Trình Quản Lý Ca

1. Admin tạo lịch mẫu, cấu hình ca chuẩn (sáng/trưa/tối, số lượng nhân sự tối đa).
2. Quản lý ca phân công nhân viên phục vụ, lễ tân, thu ngân, bếp.
3. Nhân viên nhận ca (confirm) trên app; thông tin ghi vào bảng `ShiftAttendance`.
4. Cuối ca, hệ thống tổng hợp doanh thu, tips, KPI, ghi nhận vắng/muộn.
5. Dữ liệu chuyển sang module payroll để tính lương và phụ cấp.

## Phân Quyền Kỹ Thuật

- Sử dụng ASP.NET Core Identity + JWT + refresh token.
- Gán role + policy-based authorization (ví dụ `RequireRole("Manager")`, `RequireClaim("permission", "orders.cancel.approve")`).
- React app sử dụng context + feature flag để ẩn chức năng không đủ quyền.
- Toàn bộ API nhạy cảm phải log action (ai, khi nào, bàn/hoá đơn nào).

