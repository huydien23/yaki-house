-- ============================================
-- YAKIHOUSE DATABASE - DỮ LIỆU MẪU THEO MÔ TẢ QUÁN THỰC TẾ
-- ============================================
-- Script này tạo dữ liệu mẫu dựa trên mô tả quán thực tế
-- - 27 bàn (13 tầng 1, 14 tầng 2)
-- - 3 loại vé buffet + 1 vé add-on
-- - 3 khu vực bếp + Quầy Bar + Quầy Line
-- - Các chương trình ưu đãi
-- ============================================

USE YakihouseDb;
GO

-- Xóa dữ liệu cũ (nếu có)
PRINT N'Đang xóa dữ liệu cũ...';
DELETE FROM OrderItemOptions;
DELETE FROM OrderItems;
DELETE FROM OrderAudits;
DELETE FROM Orders;
DELETE FROM KitchenTicketItems;
DELETE FROM KitchenTickets;
DELETE FROM Payments;
DELETE FROM BillPromotions;
DELETE FROM Bills;
DELETE FROM TableAssignments;
DELETE FROM ShiftAttendances;
DELETE FROM PayrollEntries;
DELETE FROM PayrollSettings;
DELETE FROM Recipes;
DELETE FROM InventoryTransactions;
DELETE FROM InventoryItems;
DELETE FROM MenuOptions;
DELETE FROM MenuOptionGroups;
DELETE FROM MenuItems;
DELETE FROM MenuCategories;
DELETE FROM DiningTables;
DELETE FROM Staff;
DELETE FROM Roles;
DELETE FROM KitchenStations;
DELETE FROM Shifts;
DELETE FROM Promotions;

PRINT N'✅ Đã xóa dữ liệu cũ';

-- ============================================
-- 1. THÊM VAI TRÒ (ROLES) - THEO MÔ TẢ QUÁN
-- ============================================
PRINT N'';
PRINT N'Đang thêm vai trò...';

DECLARE @AdminRoleId UNIQUEIDENTIFIER = NEWID();
DECLARE @ManagerRoleId UNIQUEIDENTIFIER = NEWID();
DECLARE @StaffRoleId UNIQUEIDENTIFIER = NEWID();
DECLARE @KitchenRoleId UNIQUEIDENTIFIER = NEWID();
DECLARE @CashierRoleId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Roles (Id, Name, Description, CreatedAt) VALUES
(@AdminRoleId, N'Admin', N'Admin Hệ Thống - Quản lý, vận hành và hỗ trợ toàn bộ hệ thống phần mềm', GETUTCDATE()),
(@ManagerRoleId, N'Manager', N'Quản Lý Ca - Quản lý toàn bộ hoạt động vận hành của quán', GETUTCDATE()),
(@StaffRoleId, N'Staff', N'Nhân Viên Phục Vụ - Phục vụ khách hàng tại bàn', GETUTCDATE()),
(@KitchenRoleId, N'Kitchen', N'Nhân Viên Bếp - Chế biến món ăn tại các khu vực bếp', GETUTCDATE()),
(@CashierRoleId, N'Cashier', N'Thu Ngân - Xử lý thanh toán và báo cáo doanh thu', GETUTCDATE());

PRINT N'✅ Đã thêm 5 vai trò';

-- ============================================
-- 2. THÊM NHÂN VIÊN (STAFF)
-- ============================================
PRINT N'Đang thêm nhân viên...';

DECLARE @Staff1Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Staff2Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Staff3Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Staff4Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Kitchen1Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Kitchen2Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Kitchen3Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Cashier1Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Manager1Id UNIQUEIDENTIFIER = NEWID();

INSERT INTO Staff (Id, FullName, RoleId, Phone, Email, Status, CreatedAt) VALUES
(@Staff1Id, N'Nguyễn Văn An', @StaffRoleId, '0901234567', 'vanan@yakihouse.com', 'Active', GETUTCDATE()),
(@Staff2Id, N'Trần Thị Bích', @StaffRoleId, '0909876543', 'tranb@yakihouse.com', 'Active', GETUTCDATE()),
(@Staff3Id, N'Lê Minh Cường', @StaffRoleId, '0912345678', 'cuonglm@yakihouse.com', 'Active', GETUTCDATE()),
(@Staff4Id, N'Phạm Thị Dung', @StaffRoleId, '0923456789', 'dungpt@yakihouse.com', 'Active', GETUTCDATE()),
(@Kitchen1Id, N'Hoàng Văn Em', @KitchenRoleId, '0934567890', 'emhv@yakihouse.com', 'Active', GETUTCDATE()),
(@Kitchen2Id, N'Vũ Thị Phương', @KitchenRoleId, '0945678901', 'phuongvt@yakihouse.com', 'Active', GETUTCDATE()),
(@Kitchen3Id, N'Đỗ Văn Giang', @KitchenRoleId, '0956789012', 'giangdv@yakihouse.com', 'Active', GETUTCDATE()),
(@Cashier1Id, N'Bùi Thị Hoa', @CashierRoleId, '0967890123', 'hoabt@yakihouse.com', 'Active', GETUTCDATE()),
(@Manager1Id, N'Nguyễn Văn Quản', @ManagerRoleId, '0978901234', 'quannv@yakihouse.com', 'Active', GETUTCDATE());

PRINT N'✅ Đã thêm 9 nhân viên';

-- ============================================
-- 3. THÊM BÀN (DINING TABLES) - THEO SƠ ĐỒ THỰC TẾ
-- ============================================
PRINT N'Đang thêm bàn theo sơ đồ thực tế...';

-- Tầng 1: 13 bàn (101-114, bỏ 113)
-- Bàn 6 chỗ: 105, 107, 108
-- Bàn 4 chỗ: 101, 102, 103, 104, 106, 109, 110, 111, 112, 114

INSERT INTO DiningTables (Id, Code, Zone, Capacity, Status, CreatedAt) VALUES
-- Tầng 1 - Bàn 4 chỗ
(NEWID(), N'101', N'Tầng 1', 4, 'Available', GETUTCDATE()),
(NEWID(), N'102', N'Tầng 1', 4, 'Available', GETUTCDATE()),
(NEWID(), N'103', N'Tầng 1', 4, 'Available', GETUTCDATE()),
(NEWID(), N'104', N'Tầng 1', 4, 'Available', GETUTCDATE()),
(NEWID(), N'106', N'Tầng 1', 4, 'Available', GETUTCDATE()),
(NEWID(), N'109', N'Tầng 1', 4, 'Available', GETUTCDATE()),
(NEWID(), N'110', N'Tầng 1', 4, 'Available', GETUTCDATE()),
(NEWID(), N'111', N'Tầng 1', 4, 'Available', GETUTCDATE()),
(NEWID(), N'112', N'Tầng 1', 4, 'Available', GETUTCDATE()),
(NEWID(), N'114', N'Tầng 1', 4, 'Available', GETUTCDATE()),

-- Tầng 1 - Bàn 6 chỗ
(NEWID(), N'105', N'Tầng 1', 6, 'Available', GETUTCDATE()),
(NEWID(), N'107', N'Tầng 1', 6, 'Available', GETUTCDATE()),
(NEWID(), N'108', N'Tầng 1', 6, 'Available', GETUTCDATE()),

-- Tầng 2: 14 bàn (201-215, bỏ 213)
-- Bàn 6 chỗ: 201, 202, 214
-- Bàn 2 chỗ: 206, 207
-- Bàn 4 chỗ: 203, 204, 205, 208, 209, 210, 211, 212, 215

-- Tầng 2 - Bàn 4 chỗ
(NEWID(), N'203', N'Tầng 2', 4, 'Available', GETUTCDATE()),
(NEWID(), N'204', N'Tầng 2', 4, 'Available', GETUTCDATE()),
(NEWID(), N'205', N'Tầng 2', 4, 'Available', GETUTCDATE()),
(NEWID(), N'208', N'Tầng 2', 4, 'Available', GETUTCDATE()),
(NEWID(), N'209', N'Tầng 2', 4, 'Available', GETUTCDATE()),
(NEWID(), N'210', N'Tầng 2', 4, 'Available', GETUTCDATE()),
(NEWID(), N'211', N'Tầng 2', 4, 'Available', GETUTCDATE()),
(NEWID(), N'212', N'Tầng 2', 4, 'Available', GETUTCDATE()),
(NEWID(), N'215', N'Tầng 2', 4, 'Available', GETUTCDATE()),

-- Tầng 2 - Bàn 6 chỗ
(NEWID(), N'201', N'Tầng 2', 6, 'Available', GETUTCDATE()),
(NEWID(), N'202', N'Tầng 2', 6, 'Available', GETUTCDATE()),
(NEWID(), N'214', N'Tầng 2', 6, 'Available', GETUTCDATE()),

-- Tầng 2 - Bàn 2 chỗ
(NEWID(), N'206', N'Tầng 2', 2, 'Available', GETUTCDATE()),
(NEWID(), N'207', N'Tầng 2', 2, 'Available', GETUTCDATE());

PRINT N'✅ Đã thêm 27 bàn (13 tầng 1, 14 tầng 2)';

-- ============================================
-- 4. THÊM KITCHEN STATIONS - THEO MÔ TẢ BẾP
-- ============================================
PRINT N'Đang thêm trạm bếp...';

DECLARE @BepThitId UNIQUEIDENTIFIER = NEWID();
DECLARE @BepNongId UNIQUEIDENTIFIER = NEWID();
DECLARE @BepDoAnKemId UNIQUEIDENTIFIER = NEWID();
DECLARE @QuayBarId UNIQUEIDENTIFIER = NEWID();

INSERT INTO KitchenStations (Id, Name, DisplayOrder, IsActive, CreatedAt) VALUES
(@BepThitId, N'Bếp Thịt', 1, 1, GETUTCDATE()),
(@BepNongId, N'Bếp Nóng', 2, 1, GETUTCDATE()),
(@BepDoAnKemId, N'Bếp Đồ ăn kèm', 3, 1, GETUTCDATE()),
(@QuayBarId, N'Quầy Bar', 4, 1, GETUTCDATE());

PRINT N'✅ Đã thêm 4 trạm (3 bếp + 1 quầy Bar)';

-- ============================================
-- 5. THÊM DANH MỤC MÓN ĂN
-- ============================================
PRINT N'Đang thêm danh mục món ăn...';

DECLARE @ThitTuoiId UNIQUEIDENTIFIER = NEWID();
DECLARE @HaiSanId UNIQUEIDENTIFIER = NEWID();
DECLARE @DoAnSanId UNIQUEIDENTIFIER = NEWID();
DECLARE @DoAnKemId UNIQUEIDENTIFIER = NEWID();
DECLARE @DoUongId UNIQUEIDENTIFIER = NEWID();
DECLARE @TrangMiengId UNIQUEIDENTIFIER = NEWID();

INSERT INTO MenuCategories (Id, Name, DisplayOrder, IsActive, CreatedAt) VALUES
(@ThitTuoiId, N'Thịt Tươi', 1, 1, GETUTCDATE()),
(@HaiSanId, N'Hải Sản', 2, 1, GETUTCDATE()),
(@DoAnSanId, N'Đồ Ăn Sẵn', 3, 1, GETUTCDATE()),
(@DoAnKemId, N'Đồ Ăn Kèm', 4, 1, GETUTCDATE()),
(@DoUongId, N'Đồ Uống', 5, 1, GETUTCDATE()),
(@TrangMiengId, N'Tráng Miệng', 6, 1, GETUTCDATE());

PRINT N'✅ Đã thêm 6 danh mục';

-- ============================================
-- 6. THÊM MÓN ĂN - THỰC TẾ BUFFET NƯỚNG
-- ============================================
PRINT N'Đang thêm món ăn...';

-- Thịt tươi (Bếp Thịt)
INSERT INTO MenuItems (Id, CategoryId, Name, Description, BasePrice, Status, CreatedAt) VALUES
(NEWID(), @ThitTuoiId, N'Thịt Bò Mỹ', N'Thịt bò nhập khẩu Mỹ cao cấp', 0, 'Available', GETUTCDATE()),
(NEWID(), @ThitTuoiId, N'Thịt Bò Úc', N'Thịt bò Úc tươi ngon', 0, 'Available', GETUTCDATE()),
(NEWID(), @ThitTuoiId, N'Thịt Heo Ba Chỉ', N'Ba chỉ heo tươi ngon', 0, 'Available', GETUTCDATE()),
(NEWID(), @ThitTuoiId, N'Sườn Heo', N'Sườn heo tươi', 0, 'Available', GETUTCDATE()),
(NEWID(), @ThitTuoiId, N'Thịt Gà', N'Đùi gà, cánh gà tươi', 0, 'Available', GETUTCDATE()),
(NEWID(), @ThitTuoiId, N'Xúc Xích Đức', N'Xúc xích nhập khẩu Đức', 0, 'Available', GETUTCDATE());

-- Hải sản (Bếp Thịt)
INSERT INTO MenuItems (Id, CategoryId, Name, Description, BasePrice, Status, CreatedAt) VALUES
(NEWID(), @HaiSanId, N'Tôm Sú', N'Tôm sú tươi ngon', 0, 'Available', GETUTCDATE()),
(NEWID(), @HaiSanId, N'Mực', N'Mực tươi', 0, 'Available', GETUTCDATE()),
(NEWID(), @HaiSanId, N'Cá Hồi', N'Cá hồi Na Uy nhập khẩu', 0, 'Available', GETUTCDATE()),
(NEWID(), @HaiSanId, N'Sò Điệp', N'Sò điệp tươi', 0, 'Available', GETUTCDATE()),
(NEWID(), @HaiSanId, N'Nghêu', N'Nghêu tươi', 0, 'Available', GETUTCDATE()),
(NEWID(), @HaiSanId, N'Cua', N'Cua tươi', 0, 'Available', GETUTCDATE());

-- Đồ ăn sẵn (Bếp Nóng)
INSERT INTO MenuItems (Id, CategoryId, Name, Description, BasePrice, Status, CreatedAt) VALUES
(NEWID(), @DoAnSanId, N'Lẩu Thái', N'Lẩu Thái chua cay (cho vé Buffet Nướng Lẩu)', 0, 'Available', GETUTCDATE()),
(NEWID(), @DoAnSanId, N'Lẩu Kim Chi', N'Lẩu Kim Chi (cho vé Buffet Nướng Lẩu)', 0, 'Available', GETUTCDATE()),
(NEWID(), @DoAnSanId, N'Lẩu Chua Cay', N'Lẩu chua cay (cho vé Buffet Nướng Lẩu)', 0, 'Available', GETUTCDATE()),
(NEWID(), @DoAnSanId, N'Cơm', N'Cơm trắng', 0, 'Available', GETUTCDATE()),
(NEWID(), @DoAnSanId, N'Canh', N'Canh theo ngày', 0, 'Available', GETUTCDATE());

-- Đồ ăn kèm (Bếp Đồ ăn kèm)
INSERT INTO MenuItems (Id, CategoryId, Name, Description, BasePrice, Status, CreatedAt) VALUES
(NEWID(), @DoAnKemId, N'Rau Củ Nướng', N'Hỗn hợp rau củ tươi', 0, 'Available', GETUTCDATE()),
(NEWID(), @DoAnKemId, N'Nấm Nướng', N'Nấm các loại', 0, 'Available', GETUTCDATE()),
(NEWID(), @DoAnKemId, N'Ngô Nướng', N'Ngô nướng bơ tỏi', 0, 'Available', GETUTCDATE()),
(NEWID(), @DoAnKemId, N'Khoai Lang Nướng', N'Khoai lang Nhật', 0, 'Available', GETUTCDATE()),
(NEWID(), @DoAnKemId, N'Salad', N'Salad tươi', 0, 'Available', GETUTCDATE()),
(NEWID(), @DoAnKemId, N'Panchan', N'Đồ chua Hàn Quốc', 0, 'Available', GETUTCDATE());

-- Đồ uống (Quầy Bar - gọi món riêng, tính phí)
INSERT INTO MenuItems (Id, CategoryId, Name, Description, BasePrice, Status, CreatedAt) VALUES
(NEWID(), @DoUongId, N'Nước Ngọt', N'Coca, Pepsi, 7Up, Sprite (gọi món riêng)', 15000, 'Available', GETUTCDATE()),
(NEWID(), @DoUongId, N'Bia Tiger', N'Bia Tiger lon/chai', 25000, 'Available', GETUTCDATE()),
(NEWID(), @DoUongId, N'Bia Heineken', N'Bia Heineken lon/chai', 28000, 'Available', GETUTCDATE()),
(NEWID(), @DoUongId, N'Nước Cam Ép', N'Cam tươi ép', 25000, 'Available', GETUTCDATE()),
(NEWID(), @DoUongId, N'Nước Dưa Hấu Ép', N'Dưa hấu tươi ép', 20000, 'Available', GETUTCDATE()),
(NEWID(), @DoUongId, N'Trà Đá', N'Trà đá (miễn phí)', 0, 'Available', GETUTCDATE());

-- Tráng miệng (Quầy Line - tự phục vụ cho vé Buffet Tráng Miệng)
INSERT INTO MenuItems (Id, CategoryId, Name, Description, BasePrice, Status, CreatedAt) VALUES
(NEWID(), @TrangMiengId, N'Kem', N'Kem các vị (Quầy Line)', 0, 'Available', GETUTCDATE()),
(NEWID(), @TrangMiengId, N'Hoa Quả', N'Đĩa hoa quả theo ngày (Quầy Line)', 0, 'Available', GETUTCDATE()),
(NEWID(), @TrangMiengId, N'Yaourt', N'Yaourt tự nhiên (Quầy Line)', 0, 'Available', GETUTCDATE()),
(NEWID(), @TrangMiengId, N'Nước Ngọt Buffet', N'Nước ngọt tự lấy (Quầy Line - chỉ với vé Buffet Tráng Miệng)', 0, 'Available', GETUTCDATE());

PRINT N'✅ Đã thêm món ăn';

-- ============================================
-- 7. THÊM VÉ BUFFET VÀO MENU ITEMS
-- ============================================
PRINT N'Đang thêm vé buffet vào menu...';

-- Tạo category cho vé buffet
DECLARE @VeBuffetId UNIQUEIDENTIFIER = NEWID();
INSERT INTO MenuCategories (Id, Name, DisplayOrder, IsActive, CreatedAt) VALUES
(@VeBuffetId, N'Vé Buffet', 0, 1, GETUTCDATE());

-- Thêm vé buffet như menu items
DECLARE @VeNuongId UNIQUEIDENTIFIER = NEWID();
DECLARE @VeNuongLauId UNIQUEIDENTIFIER = NEWID();
DECLARE @VeTreEmId UNIQUEIDENTIFIER = NEWID();
DECLARE @VeTrangMiengId UNIQUEIDENTIFIER = NEWID();

INSERT INTO MenuItems (Id, CategoryId, Name, Description, BasePrice, Status, CreatedAt) VALUES
(@VeNuongId, @VeBuffetId, N'Vé Buffet Nướng', N'Vé buffet nướng - 175.000 VNĐ/khách', 175000, 'Available', GETUTCDATE()),
(@VeNuongLauId, @VeBuffetId, N'Vé Buffet Nướng Lẩu', N'Vé buffet nướng lẩu - 199.000 VNĐ/khách', 199000, 'Available', GETUTCDATE()),
(@VeTreEmId, @VeBuffetId, N'Vé Buffet Trẻ Em', N'Vé buffet trẻ em (1m - 1.3m) - 79.000 VNĐ/khách', 79000, 'Available', GETUTCDATE()),
(@VeTrangMiengId, @VeBuffetId, N'Vé Buffet Tráng Miệng', N'Vé buffet tráng miệng (Add-on) - 45.000 VNĐ/khách', 45000, 'Available', GETUTCDATE());

PRINT N'✅ Đã thêm 4 loại vé buffet';

-- ============================================
-- 8. THÊM PROMOTIONS (CHƯƠNG TRÌNH ƯU ĐÃI)
-- ============================================
PRINT N'Đang thêm chương trình ưu đãi...';

-- Ưu đãi theo nhóm
DECLARE @PromoDi4Tinh3Id UNIQUEIDENTIFIER = NEWID();
DECLARE @PromoDi4Giam100kId UNIQUEIDENTIFIER = NEWID();

-- Ưu đãi hàng tuần
DECLARE @PromoDatBanTruocId UNIQUEIDENTIFIER = NEWID();
DECLARE @PromoGioVangId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Promotions (Id, Name, Type, Value, StartDate, EndDate, Conditions, IsActive, CreatedAt) VALUES
-- Đi 4 Tính 3 (lũy tiến: 8 tính 6, 12 tính 9...)
(@PromoDi4Tinh3Id, N'Đi 4 Tính 3', N'GroupDiscount', 25000, '2024-01-01', '2099-12-31', 
 N'Áp dụng lũy tiến. Không áp dụng cuối tuần và ngày lễ', 1, GETUTCDATE()),

-- Đi 4 Giảm 100.000 VNĐ (lũy tiến: 8 giảm 200k...)
(@PromoDi4Giam100kId, N'Đi 4 Giảm 100k', N'GroupDiscount', 100000, '2024-01-01', '2099-12-31',
 N'Áp dụng lũy tiến. Không áp dụng cuối tuần và ngày lễ', 1, GETUTCDATE()),

-- Đặt bàn trước (Thứ 2 - Thứ 6)
(@PromoDatBanTruocId, N'Đặt Bàn Trước', N'FixedDiscount', 50000, '2024-01-01', '2099-12-31',
 N'Giảm 50.000 VNĐ cho nhóm đặt trước từ 4 khách. Chỉ áp dụng Thứ 2 - Thứ 6', 1, GETUTCDATE()),

-- Giờ Vàng (Thứ 2 - Thứ 6, 11:00 - 17:00)
(@PromoGioVangId, N'Giờ Vàng', N'PerPersonDiscount', 20000, '2024-01-01', '2099-12-31',
 N'Giảm 20.000 VNĐ/người. Áp dụng 11:00 - 17:00. Chỉ áp dụng Thứ 2 - Thứ 6', 1, GETUTCDATE());

PRINT N'✅ Đã thêm 4 chương trình ưu đãi';

-- ============================================
-- 9. THÊM SHIFTS (CA LÀM VIỆC)
-- ============================================
PRINT N'Đang thêm ca làm việc...';

DECLARE @CaSangId UNIQUEIDENTIFIER = NEWID();
DECLARE @CaChieuId UNIQUEIDENTIFIER = NEWID();
DECLARE @CaToiId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Shifts (Id, Name, StartTime, EndTime, ShiftType, Status, CreatedAt) VALUES
(@CaSangId, N'Ca Sáng', '06:00:00', '14:00:00', 'Morning', 0, GETUTCDATE()),
(@CaChieuId, N'Ca Chiều', '14:00:00', '22:00:00', 'Afternoon', 0, GETUTCDATE()),
(@CaToiId, N'Ca Tối', '18:00:00', '23:00:00', 'Evening', 0, GETUTCDATE());

PRINT N'✅ Đã thêm 3 ca làm việc';

-- ============================================
-- 10. PAYROLL SETTINGS (THIẾT LẬP LƯƠNG)
-- ============================================
PRINT N'Đang thiết lập mức lương...';

INSERT INTO PayrollSettings (Id, RoleId, BaseRate, Allowance, CreatedAt) VALUES
(NEWID(), @AdminRoleId, 15000000, 3000000, GETUTCDATE()),
(NEWID(), @ManagerRoleId, 12000000, 2000000, GETUTCDATE()),
(NEWID(), @StaffRoleId, 6000000, 1000000, GETUTCDATE()),
(NEWID(), @KitchenRoleId, 7000000, 1200000, GETUTCDATE()),
(NEWID(), @CashierRoleId, 6500000, 1000000, GETUTCDATE());

PRINT N'✅ Đã thiết lập mức lương';

-- ============================================
-- 11. KIỂM TRA DỮ LIỆU
-- ============================================
PRINT N'';
PRINT N'============================================';
PRINT N'TỔNG KẾT DỮ LIỆU ĐÃ THÊM:';
PRINT N'============================================';

SELECT 'Roles' as [Bảng], COUNT(*) as [Số lượng] FROM Roles
UNION ALL
SELECT 'Staff', COUNT(*) FROM Staff
UNION ALL
SELECT 'DiningTables', COUNT(*) FROM DiningTables
UNION ALL
SELECT 'MenuCategories', COUNT(*) FROM MenuCategories
UNION ALL
SELECT 'MenuItems', COUNT(*) FROM MenuItems
UNION ALL
SELECT 'KitchenStations', COUNT(*) FROM KitchenStations
UNION ALL
SELECT 'Shifts', COUNT(*) FROM Shifts
UNION ALL
SELECT 'Promotions', COUNT(*) FROM Promotions
UNION ALL
SELECT 'PayrollSettings', COUNT(*) FROM PayrollSettings;

PRINT N'';
PRINT N'✅ HOÀN TẤT! Dữ liệu đã được thêm theo mô tả quán thực tế.';
PRINT N'';
PRINT N'📊 CHI TIẾT:';
PRINT N'   - 27 bàn: 13 tầng 1, 14 tầng 2';
PRINT N'   - 4 loại vé buffet (175k, 199k, 79k, 45k)';
PRINT N'   - 4 trạm (3 bếp + 1 quầy Bar)';
PRINT N'   - 4 chương trình ưu đãi';
PRINT N'   - Menu đầy đủ theo buffet nướng';
PRINT N'';
PRINT N'🎉 Bạn có thể bắt đầu test hệ thống ngay bây giờ!';
PRINT N'';

GO
