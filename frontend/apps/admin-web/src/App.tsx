import { Routes, Route, Navigate, Link } from 'react-router-dom';
import { useAuthStore } from './stores/authStore';
import Login from './pages/Login';
import Dashboard from './pages/Dashboard';
import MenuManagement from './pages/MenuManagement';
import StaffManagement from './pages/StaffManagement';
import TablesManagement from './pages/TablesManagement';
import Reports from './pages/Reports';

function App() {
  const { isAuthenticated, user, logout } = useAuthStore();

  if (!isAuthenticated) {
    return (
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="*" element={<Navigate to="/login" replace />} />
      </Routes>
    );
  }

  return (
    <div className="flex h-screen bg-gray-100">
      {/* Sidebar */}
      <aside className="w-64 bg-primary-800 text-white">
        <div className="p-6">
          <h1 className="text-2xl font-bold">Yakihouse</h1>
          <p className="text-sm text-primary-200">Admin Console</p>
        </div>

        <nav className="mt-6">
          <Link
            to="/admin"
            className="flex items-center px-6 py-3 hover:bg-primary-700 transition-colors"
          >
            <span className="mr-3">📊</span>
            <span>Dashboard</span>
          </Link>
          <Link
            to="/admin/menu"
            className="flex items-center px-6 py-3 hover:bg-primary-700 transition-colors"
          >
            <span className="mr-3">🍱</span>
            <span>Menu</span>
          </Link>
          <Link
            to="/admin/staff"
            className="flex items-center px-6 py-3 hover:bg-primary-700 transition-colors"
          >
            <span className="mr-3">👥</span>
            <span>Nhân viên</span>
          </Link>
          <Link
            to="/admin/tables"
            className="flex items-center px-6 py-3 hover:bg-primary-700 transition-colors"
          >
            <span className="mr-3">🪑</span>
            <span>Bàn ăn</span>
          </Link>
          <Link
            to="/admin/reports"
            className="flex items-center px-6 py-3 hover:bg-primary-700 transition-colors"
          >
            <span className="mr-3">📈</span>
            <span>Báo cáo</span>
          </Link>
        </nav>
      </aside>

      {/* Main Content */}
      <div className="flex-1 flex flex-col overflow-hidden">
        {/* Header */}
        <header className="bg-white shadow-sm">
          <div className="px-6 py-4 flex items-center justify-between">
            <h2 className="text-xl font-semibold text-gray-800">
              Chào mừng, {user?.fullName}
            </h2>
            <button
              onClick={logout}
              className="px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700"
            >
              Đăng xuất
            </button>
          </div>
        </header>

        {/* Content */}
        <main className="flex-1 overflow-auto">
          <Routes>
            <Route path="/admin" element={<Dashboard />} />
            <Route path="/admin/menu" element={<MenuManagement />} />
            <Route path="/admin/staff" element={<StaffManagement />} />
            <Route path="/admin/tables" element={<TablesManagement />} />
            <Route path="/admin/reports" element={<Reports />} />
            <Route path="*" element={<Navigate to="/admin" replace />} />
          </Routes>
        </main>
      </div>
    </div>
  );
}

export default App;

