# 📋 HƯỚNG DẪN CHẠY SCRIPT SQL

## 🎯 Mục đích
Script `database_sample_data.sql` tạo dữ liệu mẫu theo đúng mô tả quán thực tế:
- 27 bàn (13 tầng 1, 14 tầng 2)
- 4 loại vé buffet (175k, 199k, 79k, 45k)
- 4 trạm bếp (3 bếp + 1 quầy Bar)
- 4 chương trình ưu đãi
- Menu đầy đủ theo buffet nướng

---

## 📝 CÁCH CHẠY SCRIPT

### Cách 1: Dùng SQL Server Management Studio (SSMS)

1. **Mở SQL Server Management Studio**
   - Connect tới server: `HUYDIEN`
   - Database: `YakihouseDb`

2. **Mở file SQL**
   - File → Open → File
   - Chọn file: `database_sample_data.sql`

3. **Chạy script**
   - Nhấn `F5` hoặc click `Execute`
   - Đợi script chạy xong (khoảng 5-10 giây)

4. **Kiểm tra kết quả**
   - Xem output window để xác nhận dữ liệu đã được thêm
   - Chạy query để kiểm tra:
   ```sql
   SELECT 'Tables' as Entity, COUNT(*) as Count FROM DiningTables
   UNION ALL
   SELECT 'MenuItems', COUNT(*) FROM MenuItems
   UNION ALL
   SELECT 'Promotions', COUNT(*) FROM Promotions;
   ```

### Cách 2: Dùng SQLCMD (Command Line)

```powershell
# Chạy script từ command line
sqlcmd -S HUYDIEN -d YakihouseDb -i database_sample_data.sql
```

### Cách 3: Dùng Azure Data Studio

1. Mở Azure Data Studio
2. Connect tới `HUYDIEN` → `YakihouseDb`
3. Mở file `database_sample_data.sql`
4. Chạy script (F5)

---

## ✅ KIỂM TRA SAU KHI CHẠY

### 1. Kiểm tra bàn
```sql
-- Xem danh sách bàn
SELECT Code, Zone, Capacity, Status 
FROM DiningTables 
ORDER BY Zone, Code;

-- Đếm số bàn theo tầng
SELECT Zone, COUNT(*) as SoBan
FROM DiningTables
GROUP BY Zone;
```

**Kết quả mong đợi:**
- Tầng 1: 13 bàn
- Tầng 2: 14 bàn
- Tổng: 27 bàn

### 2. Kiểm tra vé buffet
```sql
-- Xem các vé buffet
SELECT Name, BasePrice, Description
FROM MenuItems
WHERE CategoryId IN (
    SELECT Id FROM MenuCategories WHERE Name = N'Vé Buffet'
)
ORDER BY BasePrice DESC;
```

**Kết quả mong đợi:**
- Vé Buffet Nướng Lẩu: 199.000 VNĐ
- Vé Buffet Nướng: 175.000 VNĐ
- Vé Buffet Trẻ Em: 79.000 VNĐ
- Vé Buffet Tráng Miệng: 45.000 VNĐ

### 3. Kiểm tra bếp
```sql
-- Xem các trạm bếp
SELECT Name, DisplayOrder, IsActive
FROM KitchenStations
ORDER BY DisplayOrder;
```

**Kết quả mong đợi:**
- Bếp Thịt
- Bếp Nóng
- Bếp Đồ ăn kèm
- Quầy Bar

### 4. Kiểm tra ưu đãi
```sql
-- Xem các chương trình ưu đãi
SELECT Name, Type, Value, Conditions, IsActive
FROM Promotions
ORDER BY Name;
```

**Kết quả mong đợi:**
- Đi 4 Tính 3
- Đi 4 Giảm 100k
- Đặt Bàn Trước
- Giờ Vàng

### 5. Kiểm tra menu
```sql
-- Xem số lượng món ăn theo danh mục
SELECT c.Name as DanhMuc, COUNT(m.Id) as SoMon
FROM MenuCategories c
LEFT JOIN MenuItems m ON c.Id = m.CategoryId
WHERE c.Name != N'Vé Buffet'
GROUP BY c.Name
ORDER BY c.DisplayOrder;
```

---

## 🔄 XÓA VÀ CHẠY LẠI

Nếu muốn xóa dữ liệu cũ và chạy lại script:

### Cách 1: Chạy script (đã có phần xóa)
Script đã có phần xóa dữ liệu cũ ở đầu, chỉ cần chạy lại script.

### Cách 2: Xóa thủ công
```sql
-- Xóa dữ liệu theo thứ tự (để tránh foreign key constraint)
DELETE FROM OrderItemOptions;
DELETE FROM OrderItems;
DELETE FROM Orders;
DELETE FROM Bills;
DELETE FROM DiningTables;
DELETE FROM MenuItems;
DELETE FROM MenuCategories;
DELETE FROM Staff;
DELETE FROM Roles;
-- ... (xóa các bảng khác)
```

---

## 🐛 XỬ LÝ LỖI

### Lỗi 1: "Cannot insert duplicate key"
**Nguyên nhân**: Dữ liệu đã tồn tại
**Giải pháp**: 
- Chạy phần xóa dữ liệu cũ trong script
- Hoặc xóa thủ công như trên

### Lỗi 2: "Foreign key constraint"
**Nguyên nhân**: Xóa dữ liệu không đúng thứ tự
**Giải pháp**: 
- Script đã xử lý đúng thứ tự
- Nếu vẫn lỗi, kiểm tra lại các bảng có foreign key

### Lỗi 3: "Invalid object name"
**Nguyên nhân**: Database chưa có bảng (migrations chưa chạy)
**Giải pháp**: 
```bash
cd backend
dotnet ef database update --project src\Yakihouse.Infrastructure --startup-project src\Yakihouse.Api
```

---

## 📊 DỮ LIỆU ĐƯỢC TẠO

Sau khi chạy script thành công, bạn sẽ có:

### ✅ Roles (5)
- Admin
- Manager
- Staff
- Kitchen
- Cashier

### ✅ Staff (9)
- 4 nhân viên phục vụ
- 3 nhân viên bếp
- 1 thu ngân
- 1 quản lý

### ✅ DiningTables (27)
- 13 bàn tầng 1
- 14 bàn tầng 2
- Đúng sức chứa (2, 4, 6 chỗ)

### ✅ MenuCategories (7)
- Vé Buffet
- Thịt Tươi
- Hải Sản
- Đồ Ăn Sẵn
- Đồ Ăn Kèm
- Đồ Uống
- Tráng Miệng

### ✅ MenuItems (~35)
- 4 vé buffet
- ~30 món ăn

### ✅ KitchenStations (4)
- Bếp Thịt
- Bếp Nóng
- Bếp Đồ ăn kèm
- Quầy Bar

### ✅ Promotions (4)
- Đi 4 Tính 3
- Đi 4 Giảm 100k
- Đặt Bàn Trước
- Giờ Vàng

### ✅ Shifts (3)
- Ca Sáng
- Ca Chiều
- Ca Tối

### ✅ PayrollSettings (5)
- Mức lương cho từng vai trò

---

## 🎉 SAU KHI CHẠY XONG

1. **Kiểm tra Backend API**
   ```bash
   cd backend/src/Yakihouse.Api
   dotnet run
   ```
   - Truy cập: `https://localhost:7001/swagger`
   - Test API: `GET /api/tables` → Sẽ thấy 27 bàn
   - Test API: `GET /api/menu/items` → Sẽ thấy menu đầy đủ

2. **Kiểm tra Frontend**
   ```bash
   cd frontend/apps/staff-pwa
   npm run dev
   ```
   - Truy cập: `http://localhost:5173`
   - Xem danh sách bàn → Sẽ thấy 27 bàn
   - Xem menu → Sẽ thấy menu đầy đủ

---

## 📞 HỖ TRỢ

Nếu gặp vấn đề:
1. Kiểm tra logs trong SQL Server
2. Kiểm tra connection string
3. Đảm bảo migrations đã chạy
4. Kiểm tra foreign key constraints

---

**Chúc bạn chạy script thành công! 🎊**

