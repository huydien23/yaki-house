# Yakihouse - Hệ thống quản lý nhà hàng buffet thông minh

Hệ thống quản lý toàn diện cho nhà hàng buffet Yakihouse tại Cần Thơ, được xây dựng với .NET Core 8 và React.

## 🎯 Tính năng chính

### ✅ Đã hoàn thành

#### Backend (.NET Core 8)
- ✅ **Domain Layer**: Entities, Enums, Repositories với DDD pattern
- ✅ **Application Layer**: CQRS với MediatR, FluentValidation
- ✅ **Infrastructure Layer**: EF Core, SQL Server, Repository pattern, Unit of Work
- ✅ **API Layer**: RESTful API với Swagger, SignalR cho realtime
- ✅ **Database**: Migrations đã tạo và apply thành công

**Modules Backend:**
- Orders Management (Tạo, cập nhật, theo dõi đơn hàng)
- Menu Management (Danh mục, món ăn, tùy chọn)
- Tables Management (Quản lý bàn, khu vực)
- SignalR Hubs (OrderHub, KitchenHub) cho realtime updates

#### Frontend (React + TypeScript + Vite)
- ✅ **Monorepo Structure**: Turborepo với 3 apps
- ✅ **Staff PWA**: App cho nhân viên phục vụ
- ✅ **API Client**: Axios với TypeScript types
- ✅ **SignalR Client**: Realtime connection service
- ✅ **State Management**: Zustand stores (Order, Menu, Table)
- ✅ **UI Components**: Tailwind CSS với custom Cerulean palette
- ✅ **Pages**: Home (Tables), Orders list

### 🚧 Đang phát triển
- Billing Module (Thanh toán, QR code, tiền mặt)
- HR & Payroll Module (Quản lý nhân viên, tính lương)
- Reporting & Analytics (Dashboard, báo cáo doanh thu)
- Admin Web App (Quản lý toàn bộ hệ thống)
- Kitchen Display System (Màn hình bếp)

## 🏗️ Kiến trúc hệ thống

```
QuanLyQuanAn/
├── backend/
│   └── src/
│       ├── Yakihouse.Domain/         # Domain entities, enums, interfaces
│       ├── Yakihouse.Application/    # Business logic, CQRS
│       ├── Yakihouse.Infrastructure/ # Data access, external services
│       └── Yakihouse.Api/            # REST API, SignalR hubs
│
├── frontend/
│   └── apps/
│       ├── admin-web/      # Admin dashboard (React)
│       ├── staff-pwa/      # Staff mobile app (PWA)
│       └── kds-display/    # Kitchen Display System
│
└── docs/                   # Tài liệu thiết kế
```

## 🚀 Cài đặt và chạy dự án

### Yêu cầu hệ thống
- .NET 8 SDK
- Node.js 18+ và npm/pnpm
- SQL Server (LocalDB hoặc Server)
- Visual Studio 2022 hoặc VS Code

### Backend

1. **Cấu hình Database**
```bash
cd backend/src/Yakihouse.Api
```

Cập nhật connection string trong `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "Default": "Server=YOUR_SERVER;Database=YakihouseDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

2. **Chạy Migrations** (đã apply rồi, chỉ cần nếu thay đổi)
```bash
cd backend
dotnet ef database update --project src\Yakihouse.Infrastructure --startup-project src\Yakihouse.Api
```

3. **Chạy Backend API**
```bash
cd backend/src/Yakihouse.Api
dotnet run
```

API sẽ chạy tại: `https://localhost:7001`
Swagger UI: `https://localhost:7001/swagger`

### Frontend

**⚠️ LƯU Ý QUAN TRỌNG**: Do thư mục dự án chứa ký tự `#` trong đường dẫn (`D:/C#/QuanLyQuanAn`), Vite có thể gặp vấn đề khi build production. Khuyến nghị:
- Đổi tên thư mục thành `D:/CSharp/QuanLyQuanAn` hoặc
- Chỉ chạy development mode (`npm run dev`)

1. **Cài đặt dependencies**
```bash
cd frontend/apps/staff-pwa
npm install
```

2. **Tạo file .env**
```bash
# frontend/apps/staff-pwa/.env
VITE_API_URL=https://localhost:7001/api
```

3. **Chạy Development Server**
```bash
npm run dev
```

Staff PWA sẽ chạy tại: `http://localhost:5173`

## 📡 API Endpoints

### Orders
- `GET /api/orders` - Lấy danh sách đơn hàng active
- `GET /api/orders/{id}` - Lấy chi tiết đơn hàng
- `POST /api/orders` - Tạo đơn hàng mới
- `PATCH /api/orders/{id}/status` - Cập nhật trạng thái đơn hàng

### Menu
- `GET /api/menu/categories` - Lấy danh mục món ăn
- `GET /api/menu/items` - Lấy danh sách món ăn
- `GET /api/menu/items/{id}` - Lấy chi tiết món ăn

### Tables
- `GET /api/tables` - Lấy danh sách bàn
- `GET /api/tables/{id}` - Lấy thông tin bàn
- `GET /api/tables/zones` - Lấy danh sách khu vực

### SignalR Hubs
- `/hubs/orders` - Hub cho order updates
- `/hubs/kitchen` - Hub cho kitchen updates

## 🎨 Màu sắc thương hiệu

Hệ thống sử dụng bảng màu Cerulean:
- Primary 50: `#EDF9FF`
- Primary 500: `#00AFFF` (Main brand color)
- Primary 900: `#004D80`
- Primary 950: `#0D2C5E`

## 📊 Database Schema

Database đã được tạo với các bảng chính:
- **Roles** - Vai trò người dùng
- **Staff** - Nhân viên
- **DiningTables** - Bàn ăn
- **MenuCategories**, **MenuItems** - Thực đơn
- **Orders**, **OrderItems** - Đơn hàng
- **KitchenTickets** - Phiếu bếp
- **Bills**, **Payments** - Hóa đơn, thanh toán
- **InventoryItems** - Kho nguyên liệu
- **PayrollEntries** - Bảng lương

Xem chi tiết ERD tại: `docs/erd.md`

## 🔧 Công nghệ sử dụng

### Backend
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8
- SQL Server
- MediatR (CQRS)
- FluentValidation
- SignalR
- Swagger/OpenAPI

### Frontend
- React 18
- TypeScript
- Vite
- Tailwind CSS
- Zustand (State Management)
- Axios
- SignalR Client
- React Router
- Turborepo (Monorepo)

## 📝 Tài liệu

- [Workflows](docs/workflows.md) - Quy trình nghiệp vụ
- [Roles & Permissions](docs/roles-and-permissions.md) - Phân quyền
- [ERD](docs/erd.md) - Sơ đồ database

## 🐛 Known Issues

1. **Vite build error với path chứa `#`**: Do Windows path chứa ký tự đặc biệt. Giải pháp: đổi tên thư mục hoặc chỉ dùng dev mode.
2. **CORS**: Đã cấu hình AllowAll cho development. Production cần cấu hình cụ thể.
3. **Authentication**: Chưa implement, đang dùng hardcoded staffId/actorId.

## 🚀 Roadmap

### Phase 1 (Hoàn thành ✅)
- ✅ Domain modeling & Database
- ✅ Order management API
- ✅ Menu & Tables API
- ✅ SignalR realtime
- ✅ Staff PWA basic UI

### Phase 2 (Đang thực hiện)
- 🚧 Billing & Payment module
- 🚧 Kitchen Display System
- 🚧 Admin Dashboard

### Phase 3 (Kế hoạch)
- ⏳ HR & Payroll
- ⏳ Inventory Management
- ⏳ Reporting & Analytics
- ⏳ Authentication & Authorization

## 👨‍💻 Development

### Chạy Backend tests
```bash
cd backend
dotnet test
```

### Chạy Frontend tests
```bash
cd frontend/apps/staff-pwa
npm run test
```

### Build Production
```bash
# Backend
cd backend
dotnet publish -c Release

# Frontend (sau khi fix path issue)
cd frontend
npm run build
```

## 📞 Hỗ trợ

Nếu gặp vấn đề, vui lòng:
1. Kiểm tra logs trong terminal
2. Xem Swagger UI để test API
3. Kiểm tra browser console cho frontend errors

## 📄 License

Private project for Yakihouse Restaurant.

---

**Developed with ❤️ for Yakihouse Cần Thơ**

