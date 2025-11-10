import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';

interface DashboardStats {
  todayRevenue: number;
  todayOrders: number;
  activeStaff: number;
  availableTables: number;
}

export default function Dashboard() {
  const [stats, setStats] = useState<DashboardStats>({
    todayRevenue: 0,
    todayOrders: 0,
    activeStaff: 0,
    availableTables: 0,
  });

  useEffect(() => {
    // Mock data - replace with actual API call
    setStats({
      todayRevenue: 15750000,
      todayOrders: 47,
      activeStaff: 12,
      availableTables: 8,
    });
  }, []);

  return (
    <div className="p-6 max-w-7xl mx-auto">
      <h1 className="text-3xl font-bold text-gray-900 mb-8">Dashboard</h1>

      {/* Stats Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
        <div className="bg-white rounded-lg shadow p-6">
          <div className="flex items-center">
            <div className="flex-shrink-0 bg-green-100 rounded-full p-3">
              <span className="text-2xl">💰</span>
            </div>
            <div className="ml-4">
              <p className="text-sm font-medium text-gray-600">Doanh thu hôm nay</p>
              <p className="text-2xl font-bold text-gray-900">
                {stats.todayRevenue.toLocaleString('vi-VN')}đ
              </p>
            </div>
          </div>
        </div>

        <div className="bg-white rounded-lg shadow p-6">
          <div className="flex items-center">
            <div className="flex-shrink-0 bg-blue-100 rounded-full p-3">
              <span className="text-2xl">📋</span>
            </div>
            <div className="ml-4">
              <p className="text-sm font-medium text-gray-600">Đơn hàng hôm nay</p>
              <p className="text-2xl font-bold text-gray-900">{stats.todayOrders}</p>
            </div>
          </div>
        </div>

        <div className="bg-white rounded-lg shadow p-6">
          <div className="flex items-center">
            <div className="flex-shrink-0 bg-purple-100 rounded-full p-3">
              <span className="text-2xl">👥</span>
            </div>
            <div className="ml-4">
              <p className="text-sm font-medium text-gray-600">Nhân viên online</p>
              <p className="text-2xl font-bold text-gray-900">{stats.activeStaff}</p>
            </div>
          </div>
        </div>

        <div className="bg-white rounded-lg shadow p-6">
          <div className="flex items-center">
            <div className="flex-shrink-0 bg-orange-100 rounded-full p-3">
              <span className="text-2xl">🪑</span>
            </div>
            <div className="ml-4">
              <p className="text-sm font-medium text-gray-600">Bàn trống</p>
              <p className="text-2xl font-bold text-gray-900">{stats.availableTables}</p>
            </div>
          </div>
        </div>
      </div>

      {/* Quick Actions */}
      <div className="bg-white rounded-lg shadow p-6 mb-8">
        <h2 className="text-xl font-bold text-gray-900 mb-4">Thao tác nhanh</h2>
        <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
          <Link
            to="/admin/menu"
            className="flex flex-col items-center p-4 border-2 border-gray-200 rounded-lg hover:border-primary-600 hover:bg-primary-50 transition-all"
          >
            <span className="text-4xl mb-2">🍱</span>
            <span className="font-medium text-gray-700">Quản lý Menu</span>
          </Link>

          <Link
            to="/admin/staff"
            className="flex flex-col items-center p-4 border-2 border-gray-200 rounded-lg hover:border-primary-600 hover:bg-primary-50 transition-all"
          >
            <span className="text-4xl mb-2">👨‍💼</span>
            <span className="font-medium text-gray-700">Nhân viên</span>
          </Link>

          <Link
            to="/admin/tables"
            className="flex flex-col items-center p-4 border-2 border-gray-200 rounded-lg hover:border-primary-600 hover:bg-primary-50 transition-all"
          >
            <span className="text-4xl mb-2">🪑</span>
            <span className="font-medium text-gray-700">Quản lý Bàn</span>
          </Link>

          <Link
            to="/admin/reports"
            className="flex flex-col items-center p-4 border-2 border-gray-200 rounded-lg hover:border-primary-600 hover:bg-primary-50 transition-all"
          >
            <span className="text-4xl mb-2">📊</span>
            <span className="font-medium text-gray-700">Báo cáo</span>
          </Link>
        </div>
      </div>

      {/* Recent Orders */}
      <div className="bg-white rounded-lg shadow p-6">
        <h2 className="text-xl font-bold text-gray-900 mb-4">Đơn hàng gần đây</h2>
        <div className="text-center text-gray-500 py-8">
          Chức năng đang được phát triển...
        </div>
      </div>
    </div>
  );
}
