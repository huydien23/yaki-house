## Tổng Quan Quy Trình

- Các actor chính: `Nhân viên phục vụ`, `Nhân viên lễ tân/thu ngân`, `Bếp trưởng & tổ bếp`, `Quản lý ca`, `Admin hệ thống`.
- Kênh giao tiếp thời gian thực: REST API (CRUD), SignalR events (đẩy trạng thái order, bàn, thanh toán).
- Mọi nghiệp vụ ghi log hoạt động để phục vụ truy vết và đối soát.

## Luồng Phục Vụ Bàn (Nhân Viên Phục Vụ)

1. Nhân viên đăng nhập và nhận ca, danh sách bàn được phân công.
2. Đón khách, cập nhật số khách, trạng thái bàn chuyển sang `Seated`.
3. Tạo order mới: chọn món, số lượng, ghi chú, ưu tiên bếp; gửi tới backend.
4. Khi khách yêu cầu điều chỉnh:
   - Tăng món: thêm mới item → backend đẩy event tới bếp.
   - Giảm/bỏ món:
     - Nếu món chưa vào bếp: cập nhật trực tiếp.
     - Nếu đang chế biến hoặc đã hoàn tất: gửi yêu cầu hủy → quản lý duyệt.
5. Theo dõi trạng thái món (SignalR):
   - `Pending` → `In Progress` (bếp nhận).
   - `Ready` (bếp hoàn tất) → nhân viên phục vụ giao cho khách.
6. Khi khách yêu cầu thanh toán: tạo yêu cầu thu ngân, bàn sang trạng thái `Awaiting Payment`.

## Luồng Bếp (Kitchen Display System)

1. KDS nhận danh sách order mới theo thời gian thực (lọc theo trạm bếp).
2. Bếp trưởng phân bổ món cho từng line cook; có thể đánh dấu ưu tiên.
3. Khi bắt đầu chế biến: chuyển item sang `In Progress` → backend cập nhật cho nhân viên & admin.
4. Hoàn tất món: chuyển sang `Ready`, có thể thêm ghi chú (ví dụ hết nguyên liệu, đề xuất thay thế).
5. Trường hợp món bị hủy:
   - Nếu yêu cầu hủy đến trước khi bắt đầu: KDS `Reject` yêu cầu (vá) hoặc `Approve` để trừ.
   - Nếu đang làm/done: cần quản lý xác nhận, hệ thống log để trừ KPI lãng phí.
6. KDS lưu lịch sử 48h gần nhất để truy lại khi có khiếu nại.

## Luồng Thanh Toán (Lễ Tân/Thu Ngân)

1. Nhận yêu cầu `Awaiting Payment`:
   - Xem chi tiết order, phụ thu, khuyến mãi, voucher.
2. Chọn phương thức thanh toán:
   - Tiền mặt: ghi nhận số tiền, tính tiền thừa.
   - QR/VietQR: tạo mã, chờ xác nhận giao dịch (giả lập webhook/scan).
   - Thẻ: nhập thông tin POS (tích hợp sau).
3. Hoàn tất: backend tạo record `Bill`, cập nhật bàn sang `Completed`, gửi thông báo cho nhân viên & admin.
4. Tùy chọn in hóa đơn, gửi email/zalo.

## Quản Lý & Admin

- Theo dõi dashboard trạng thái bàn, order, warning món hết hàng.
- Duyệt yêu cầu hủy món khi đã vào bếp/đã ready.
- Cấu hình ca làm, phân quyền, giá bán, khuyến mãi.
- Đồng bộ báo cáo cuối ca: doanh thu, số món phục vụ, KPI cá nhân.

## Trường Hợp Ngoại Lệ

- **Mất kết nối**: client lưu queue local, đồng bộ lại khi online; backend hỗ trợ idempotent requests.
- **Bàn đổi nhân viên phụ trách**: quản lý update assignment → push event tới các client liên quan.
- **Order tách/gộp bàn**: hỗ trợ chia item, tạo bill riêng; cập nhật lại mapping order-bàn.

