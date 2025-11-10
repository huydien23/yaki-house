import { useState } from 'react';

export default function Reports() {
  const [dateRange, setDateRange] = useState('today');

  return (
    <div className="p-6">
      <div className="mb-6">
        <h1 className="text-3xl font-bold text-gray-900 mb-2">Báo Cáo & Thống Kê</h1>
        <p className="text-gray-600">Phân tích doanh thu và hiệu suất</p>
      </div>

      {/* Date Filter */}
      <div className="bg-white rounded-lg shadow-sm p-4 mb-6">
        <div className="flex gap-2">
          <select
            value={dateRange}
            onChange={(e) => setDateRange(e.target.value)}
            className="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500"
          >
            <option value="today">Hôm nay</option>
            <option value="week">Tuần này</option>
            <option value="month">Tháng này</option>
            <option value="year">Năm này</option>
            <option value="custom">Tùy chỉnh</option>
          </select>
          <button className="px-4 py-2 bg-primary-600 text-white rounded-lg hover:bg-primary-700">
            📊 Xem báo cáo
          </button>
          <button className="px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700">
            📥 Xuất Excel
          </button>
        </div>
      </div>

      {/* Revenue Stats */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-4 mb-6">
        <div className="bg-gradient-to-br from-green-500 to-green-600 rounded-lg shadow-lg p-6 text-white">
          <div className="text-sm mb-2 opacity-90">Tổng doanh thu</div>
          <div className="text-3xl font-bold">45.5M</div>
          <div className="text-sm mt-2 opacity-75">+15% so với hôm qua</div>
        </div>
        <div className="bg-gradient-to-br from-blue-500 to-blue-600 rounded-lg shadow-lg p-6 text-white">
          <div className="text-sm mb-2 opacity-90">Số đơn hàng</div>
          <div className="text-3xl font-bold">127</div>
          <div className="text-sm mt-2 opacity-75">+8% so với hôm qua</div>
        </div>
        <div className="bg-gradient-to-br from-purple-500 to-purple-600 rounded-lg shadow-lg p-6 text-white">
          <div className="text-sm mb-2 opacity-90">Giá trị TB/đơn</div>
          <div className="text-3xl font-bold">358K</div>
          <div className="text-sm mt-2 opacity-75">+3% so với hôm qua</div>
        </div>
        <div className="bg-gradient-to-br from-orange-500 to-orange-600 rounded-lg shadow-lg p-6 text-white">
          <div className="text-sm mb-2 opacity-90">Số khách</div>
          <div className="text-3xl font-bold">456</div>
          <div className="text-sm mt-2 opacity-75">+12% so với hôm qua</div>
        </div>
      </div>

      {/* Charts Placeholder */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 mb-6">
        <div className="bg-white rounded-lg shadow-sm p-6">
          <h3 className="text-lg font-bold text-gray-900 mb-4">📈 Doanh Thu Theo Giờ</h3>
          <div className="h-64 bg-gray-100 rounded-lg flex items-center justify-center text-gray-500">
            Biểu đồ doanh thu (Coming Soon)
          </div>
        </div>
        <div className="bg-white rounded-lg shadow-sm p-6">
          <h3 className="text-lg font-bold text-gray-900 mb-4">🍽️ Món Bán Chạy</h3>
          <div className="space-y-3">
            <div className="flex items-center justify-between p-3 bg-gray-50 rounded">
              <div className="font-semibold">Bò Mỹ Nướng</div>
              <div className="text-primary-600 font-bold">48 món</div>
            </div>
            <div className="flex items-center justify-between p-3 bg-gray-50 rounded">
              <div className="font-semibold">Buffet Premium</div>
              <div className="text-primary-600 font-bold">35 món</div>
            </div>
            <div className="flex items-center justify-between p-3 bg-gray-50 rounded">
              <div className="font-semibold">Tôm Nướng</div>
              <div className="text-primary-600 font-bold">27 món</div>
            </div>
          </div>
        </div>
      </div>

      {/* Staff Performance */}
      <div className="bg-white rounded-lg shadow-sm p-6">
        <h3 className="text-lg font-bold text-gray-900 mb-4">👥 Hiệu Suất Nhân Viên</h3>
        <table className="w-full">
          <thead className="bg-gray-50">
            <tr>
              <th className="px-4 py-2 text-left text-sm font-semibold text-gray-700">Nhân viên</th>
              <th className="px-4 py-2 text-left text-sm font-semibold text-gray-700">Số đơn</th>
              <th className="px-4 py-2 text-left text-sm font-semibold text-gray-700">Doanh thu</th>
              <th className="px-4 py-2 text-left text-sm font-semibold text-gray-700">Đánh giá</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-200">
            <tr>
              <td className="px-4 py-3">Trần Thị Phục Vụ</td>
              <td className="px-4 py-3 font-semibold">45 đơn</td>
              <td className="px-4 py-3 text-green-600 font-semibold">16.2M</td>
              <td className="px-4 py-3">⭐⭐⭐⭐⭐</td>
            </tr>
            <tr>
              <td className="px-4 py-3">Nguyễn Văn B</td>
              <td className="px-4 py-3 font-semibold">38 đơn</td>
              <td className="px-4 py-3 text-green-600 font-semibold">13.5M</td>
              <td className="px-4 py-3">⭐⭐⭐⭐</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  );
}
