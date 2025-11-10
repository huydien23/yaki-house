# 📊 CẤU TRÚC DATABASE THEO MÔ TẢ QUÁN THỰC TẾ

## 🏢 Tổng quan quán Yakihouse

### Thông tin cơ bản
- **Tổng số bàn**: 27 bàn
- **Tầng 1**: 13 bàn (101-114, bỏ 113)
- **Tầng 2**: 14 bàn (201-215, bỏ 213)
- **Loại hình**: Buffet Nướng & Buffet Nướng Lẩu

---

## 💰 CẤU TRÚC VÉ BUFFET

### 1. Vé Buffet Chính
| Loại Vé | Giá | Mô tả |
|---------|-----|-------|
| **Vé Buffet Nướng** | 175.000 VNĐ | Vé buffet nướng cơ bản |
| **Vé Buffet Nướng Lẩu** | 199.000 VNĐ | Vé buffet nướng + lẩu |
| **Vé Buffet Trẻ Em** | 79.000 VNĐ | Cho trẻ 1m - 1.3m |

### 2. Vé Buffet Add-on
| Loại Vé | Giá | Mô tả |
|---------|-----|-------|
| **Vé Buffet Tráng Miệng** | 45.000 VNĐ | Add-on, tự phục vụ tại Quầy Line |

### 3. Quy định giá vé trẻ em
- **Dưới 1m**: Miễn phí vé buffet chính (vẫn tính vé tráng miệng nếu dùng)
- **1m - 1.3m**: Áp dụng vé trẻ em (79.000 VNĐ)
- **Trên 1.3m**: Áp dụng giá vé người lớn

### 4. Quy định tại bàn
- Mỗi bàn bắt buộc dùng chung 1 loại vé buffet chính (175k hoặc 199k) cho tất cả người lớn
- Cho phép nâng cấp từ vé 175k lên 199k
- Vé tráng miệng: Toàn bàn phải đồng ý nếu chuyển từ gọi món sang buffet tráng miệng

---

## 🎁 CHƯƠNG TRÌNH ƯU ĐÃI

### ⚠️ Lưu ý quan trọng
- **KHÔNG áp dụng** vào Thứ 7, Chủ Nhật và ngày Lễ, Tết

### 1. Ưu đãi theo nhóm
| Tên Ưu Đãi | Giá trị | Mô tả |
|------------|---------|-------|
| **Đi 4 Tính 3** | Lũy tiến | 4 tính 3, 8 tính 6, 12 tính 9... |
| **Đi 4 Giảm 100k** | 100.000 VNĐ | Lũy tiến: 8 giảm 200k, 12 giảm 300k... |

### 2. Ưu đãi hàng tuần (Thứ 2 - Thứ 6)
| Tên Ưu Đãi | Giá trị | Điều kiện |
|------------|---------|-----------|
| **Đặt Bàn Trước** | 50.000 VNĐ | Nhóm từ 4 khách, đặt trước |
| **Giờ Vàng** | 20.000 VNĐ/người | 11:00 - 17:00, giảm trực tiếp trên giá vé |

---

## 🪑 SƠ ĐỒ BÀN

### Tầng 1 (13 bàn)
**Bàn 4 chỗ (10 bàn):**
- 101, 102, 103, 104, 106, 109, 110, 111, 112, 114

**Bàn 6 chỗ (3 bàn):**
- 105, 107, 108

**Sơ đồ bố trí:**
```
View bờ hồ
105  106  107  Cầu thang

104  112  114  108

103  110  111  Quầy line

102  109      Bar

101            NVS

Bếp thịt | Bếp rau | Bếp nóng
```

### Tầng 2 (14 bàn)
**Bàn 2 chỗ (2 bàn):**
- 206, 207

**Bàn 4 chỗ (9 bàn):**
- 203, 204, 205, 208, 209, 210, 211, 212, 215

**Bàn 6 chỗ (3 bàn):**
- 201, 202, 214

**Sơ đồ bố trí:**
```
207  View bờ hồ
206  208
205  209
204  210
203  211
202  214
201  215
212
```

### Quy định ghép bàn

**Ghép 2 bàn:**
- 102 - 109
- 106 - 107
- 110 - 111
- 112 - 114
- 201 - 215
- 202 - 214

**Ghép 3 bàn:**
- 103 - 110 - 111
- 104 - 112 - 114
- 105 - 106 - 107

### Trạng thái bàn
- **Available** (Bàn Trống): Bàn sẵn sàng phục vụ
- **Occupied** (Đang Dùng): Bàn đang có khách
- **Reserved** (Đã Đặt): Bàn đã được đặt trước
- **Cleaning** (Đang Dọn): Bàn đang được dọn dẹp

**Quy trình:**
- Bàn đặt trước: Cập nhật trạng thái "Đã Đặt"
- Khách đến trễ 15-30 phút: Giải phóng về "Bàn Trống" (cần xác nhận quản lý)

---

## 🍽️ CẤU TRÚC BẾP & QUẦY

### Phân khu Bếp (Back of House)

#### 1. Bếp Thịt
- **Chức năng**: Chế biến món đồ tươi, sống (thịt, hải sản)
- **Máy in bill**: Riêng biệt
- **Món ăn**: Thịt bò, thịt heo, thịt gà, hải sản tươi

#### 2. Bếp Nóng
- **Chức năng**: Chế biến món làm sẵn, đồ ăn chín
- **Máy in bill**: Riêng biệt
- **Món ăn**: Lẩu (cho vé Nướng Lẩu), cơm, canh

#### 3. Bếp Đồ ăn kèm
- **Chức năng**: Chế biến món đồ ăn kèm
- **Máy in bill**: Riêng biệt
- **Món ăn**: Salad, panchan, rau củ nướng, đồ chiên

### Phân khu Quầy (Front of House)

#### 1. Quầy Bar
- **Chức năng**: Pha chế đồ uống (gọi món riêng)
- **Món ăn**: Đồ uống có phí (bia, nước ngọt, nước ép), tráng miệng theo yêu cầu
- **Tính phí**: Riêng (không bao gồm trong vé buffet)

#### 2. Quầy Line
- **Chức năng**: Tự phục vụ (self-service)
- **Dành cho**: Khách có vé Buffet Tráng Miệng
- **Món ăn**: Nước ngọt, kem, hoa quả, yaourt (tự lấy)

---

## 👥 CẤU TRÚC NHÂN SỰ

### Vai trò (Roles)

#### 1. Admin Hệ Thống
- Quản lý, vận hành và hỗ trợ toàn bộ hệ thống phần mềm
- Quản lý POS, máy in bill, quản lý đơn hàng
- Xử lý vấn đề kỹ thuật

#### 2. Quản Lý Ca (Shift Manager)
- Chịu trách nhiệm cao nhất trong ca
- Quản lý toàn bộ hoạt động vận hành
- Giám sát và điều phối các bộ phận
- Xử lý phàn nàn, đảm bảo chất lượng dịch vụ
- Xác nhận báo cáo kiểm kê cuối ca

#### 3. Thu Ngân (Cashier)
- Phụ trách quầy thu ngân
- Xử lý thanh toán
- Áp dụng chương trình khuyến mãi
- Quản lý và báo cáo doanh thu cuối ca

#### 4. Nhân Viên Phục Vụ (Service Staff)
- Phục vụ khách hàng tại bàn
- Tiếp nhận order
- Tư vấn menu (vé buffet, ưu đãi)
- Đảm bảo khu vực bàn ăn sạch sẽ
- **Cuối ca**: Kiểm kê và báo cáo tồn kho (đồ uống, khăn lạnh...)

#### 5. Nhân Viên Bếp (Kitchen Staff)
- Làm việc tại các khu vực bếp được phân công
- Chế biến món ăn theo order từ máy in bill
- Đảm bảo tốc độ, chất lượng, vệ sinh
- **Cuối ca**: Kiểm kê và báo cáo tồn kho nguyên vật liệu

---

## 📋 MENU ITEMS

### Danh mục món ăn

#### 1. Thịt Tươi (Bếp Thịt)
- Thịt Bò Mỹ
- Thịt Bò Úc
- Thịt Heo Ba Chỉ
- Sườn Heo
- Thịt Gà
- Xúc Xích Đức

#### 2. Hải Sản (Bếp Thịt)
- Tôm Sú
- Mực
- Cá Hồi
- Sò Điệp
- Nghêu
- Cua

#### 3. Đồ Ăn Sẵn (Bếp Nóng)
- Lẩu Thái (cho vé Nướng Lẩu)
- Lẩu Kim Chi (cho vé Nướng Lẩu)
- Lẩu Chua Cay (cho vé Nướng Lẩu)
- Cơm
- Canh

#### 4. Đồ Ăn Kèm (Bếp Đồ ăn kèm)
- Rau Củ Nướng
- Nấm Nướng
- Ngô Nướng
- Khoai Lang Nướng
- Salad
- Panchan

#### 5. Đồ Uống (Quầy Bar - tính phí riêng)
- Nước Ngọt: 15.000 VNĐ
- Bia Tiger: 25.000 VNĐ
- Bia Heineken: 28.000 VNĐ
- Nước Cam Ép: 25.000 VNĐ
- Nước Dưa Hấu Ép: 20.000 VNĐ
- Trà Đá: Miễn phí

#### 6. Tráng Miệng (Quầy Line - tự phục vụ)
- Kem (các vị)
- Hoa Quả (theo ngày)
- Yaourt
- Nước Ngọt Buffet (tự lấy)

---

## 🔄 QUY TRÌNH ORDER

### 1. Khách đến
1. Nhân viên chào khách, hướng dẫn chọn bàn
2. Kiểm tra bàn có sẵn (Available) hoặc đặt trước (Reserved)
3. Cập nhật trạng thái bàn: Available → Occupied

### 2. Tư vấn vé buffet
1. Giới thiệu các loại vé:
   - Vé Buffet Nướng (175k)
   - Vé Buffet Nướng Lẩu (199k)
   - Vé Buffet Trẻ Em (79k) - nếu có trẻ em
   - Vé Buffet Tráng Miệng (45k) - add-on
2. Kiểm tra chiều cao trẻ em (nếu có)
3. Xác nhận loại vé cho toàn bàn

### 3. Tạo Order
1. Tạo order mới với thông tin:
   - Bàn
   - Nhân viên phục vụ
   - Số khách (người lớn, trẻ em)
   - Loại vé buffet
2. Thêm vé buffet vào order items:
   - Vé người lớn: 175k hoặc 199k
   - Vé trẻ em: 79k (nếu có)
   - Vé tráng miệng: 45k (nếu có)
3. Order status: Draft

### 4. Khách order món
1. Khách chọn món từ menu (theo loại vé)
2. Nhân viên nhập order vào hệ thống
3. Order items được gửi đến các bếp tương ứng:
   - Bếp Thịt: Thịt, hải sản
   - Bếp Nóng: Lẩu (nếu vé Nướng Lẩu), cơm, canh
   - Bếp Đồ ăn kèm: Rau củ, salad, panchan
4. Máy in bill tại mỗi bếp in order

### 5. Bếp chế biến
1. Nhận order từ máy in bill
2. Chế biến món ăn
3. Cập nhật trạng thái: InProgress → Ready
4. Thông báo cho nhân viên phục vụ

### 6. Phục vụ món
1. Nhân viên phục vụ lấy món từ bếp
2. Phục vụ đến bàn
3. Cập nhật trạng thái order item: Ready → Served

### 7. Thanh toán
1. Khách yêu cầu tính tiền
2. Thu ngân xem order và tính hóa đơn:
   - Tính vé buffet (theo số khách)
   - Tính đồ uống gọi thêm (nếu có)
   - Áp dụng ưu đãi (nếu đủ điều kiện)
3. Tạo Bill với:
   - SubTotal: Tổng tiền món
   - DiscountTotal: Tổng giảm giá (ưu đãi)
   - ServiceCharge: Phí dịch vụ (nếu có)
   - Tax: Thuế (nếu có)
   - GrandTotal: Tổng thanh toán
4. Khách thanh toán (QR, tiền mặt)
5. Cập nhật Bill status: Paid
6. Cập nhật Order status: Completed
7. Cập nhật bàn: Occupied → Cleaning → Available

---

## 📊 BÁO CÁO CUỐI CA

### 1. Thu Ngân
- Tổng doanh thu
- Số lượng hóa đơn
- Phương thức thanh toán (QR, tiền mặt)
- Các chương trình ưu đãi đã áp dụng

### 2. Nhân Viên Phục Vụ
- Số lượng bàn phục vụ
- Tồn kho đồ uống, khăn lạnh...

### 3. Nhân Viên Bếp
- Tồn kho nguyên vật liệu
- Số lượng món đã chế biến

### 4. Quản Lý Ca
- Xác nhận tất cả báo cáo
- Tổng kết ca làm việc

---

## 🔧 CẤU HÌNH HỆ THỐNG

### Máy in bill
- Mỗi bếp có 1 máy in bill riêng
- Order items được phân loại và gửi đến bếp tương ứng
- In tự động khi có order mới

### SignalR Realtime
- **OrderHub**: Cập nhật order, status changes
- **KitchenHub**: Cập nhật trạng thái món, ticket status

### Tính năng đặc biệt
- **Ghép bàn**: Hệ thống hỗ trợ ghép 2-3 bàn
- **Nâng cấp vé**: Cho phép nâng cấp từ vé 175k lên 199k
- **Ưu đãi tự động**: Áp dụng ưu đãi theo điều kiện (ngày, giờ, số khách)

---

## 📝 GHI CHÚ

### Lưu ý quan trọng
1. **Ưu đãi**: Không áp dụng Thứ 7, Chủ Nhật, ngày Lễ
2. **Vé buffet**: Mỗi bàn phải dùng chung 1 loại vé chính
3. **Trẻ em**: Tính theo chiều cao, không theo tuổi
4. **Vé tráng miệng**: Toàn bàn phải đồng ý nếu chuyển từ gọi món sang buffet

### Tính năng cần phát triển
1. ✅ Quản lý bàn (đã có)
2. ✅ Order management (đã có)
3. ✅ Kitchen display (đang phát triển)
4. 🚧 Billing & Payment (đang phát triển)
5. 🚧 Promotion auto-apply (cần phát triển)
6. 🚧 Table merging (cần phát triển)
7. 🚧 End-of-shift reports (cần phát triển)

---

