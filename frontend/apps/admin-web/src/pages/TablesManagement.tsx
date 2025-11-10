import { useState, useEffect } from 'react';

interface TableDto {
  id: string;
  code: string;
  capacity: number;
  status: 'Available' | 'Occupied' | 'Reserved';
  zoneId: string;
  zoneName: string;
  qrCode?: string;
}

export default function TablesManagement() {
  const [tables, setTables] = useState<TableDto[]>([
    { id: '1', code: 'T01', capacity: 4, status: 'Available', zoneId: '1', zoneName: 'Khu A' },
    { id: '2', code: 'T02', capacity: 6, status: 'Occupied', zoneId: '1', zoneName: 'Khu A' },
    { id: '3', code: 'T03', capacity: 2, status: 'Available', zoneId: '1', zoneName: 'Khu A' },
    { id: '4', code: 'P01', capacity: 10, status: 'Reserved', zoneId: '2', zoneName: 'Phòng VIP' },
  ]);

  const getStatusBadge = (status: string) => {
    const styles = {
      Available: 'bg-green-100 text-green-700',
      Occupied: 'bg-red-100 text-red-700',
      Reserved: 'bg-yellow-100 text-yellow-700'
    };
    const labels = {
      Available: '✓ Trống',
      Occupied: '✗ Đang dùng',
      Reserved: '⏰ Đã đặt'
    };
    return { style: styles[status as keyof typeof styles], label: labels[status as keyof typeof labels] };
  };

  return (
    <div className="p-6">
      <div className="mb-6">
        <h1 className="text-3xl font-bold text-gray-900 mb-2">Quản Lý Bàn</h1>
        <p className="text-gray-600">Quản lý bàn ăn và khu vực</p>
      </div>

      {/* Stats */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-4 mb-6">
        <div className="bg-white rounded-lg shadow-sm p-4">
          <div className="text-sm text-gray-600 mb-1">Tổng số bàn</div>
          <div className="text-2xl font-bold text-gray-900">{tables.length}</div>
        </div>
        <div className="bg-white rounded-lg shadow-sm p-4">
          <div className="text-sm text-gray-600 mb-1">Bàn trống</div>
          <div className="text-2xl font-bold text-green-600">
            {tables.filter(t => t.status === 'Available').length}
          </div>
        </div>
        <div className="bg-white rounded-lg shadow-sm p-4">
          <div className="text-sm text-gray-600 mb-1">Đang sử dụng</div>
          <div className="text-2xl font-bold text-red-600">
            {tables.filter(t => t.status === 'Occupied').length}
          </div>
        </div>
        <div className="bg-white rounded-lg shadow-sm p-4">
          <div className="text-sm text-gray-600 mb-1">Đã đặt</div>
          <div className="text-2xl font-bold text-yellow-600">
            {tables.filter(t => t.status === 'Reserved').length}
          </div>
        </div>
      </div>

      {/* Tables Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
        {tables.map(table => {
          const statusBadge = getStatusBadge(table.status);
          return (
            <div key={table.id} className="bg-white rounded-lg shadow-sm p-6 hover:shadow-md transition">
              <div className="flex items-center justify-between mb-4">
                <div className="text-2xl font-bold text-gray-900">{table.code}</div>
                <span className={`px-3 py-1 rounded-full text-xs font-semibold ${statusBadge.style}`}>
                  {statusBadge.label}
                </span>
              </div>
              <div className="space-y-2 text-sm text-gray-600">
                <div>👥 Sức chứa: <span className="font-semibold">{table.capacity} người</span></div>
                <div>📍 Khu vực: <span className="font-semibold">{table.zoneName}</span></div>
              </div>
              <div className="mt-4 flex gap-2">
                <button className="flex-1 px-3 py-2 bg-blue-600 text-white text-sm rounded hover:bg-blue-700">
                  ✏️ Sửa
                </button>
                <button className="px-3 py-2 bg-gray-600 text-white text-sm rounded hover:bg-gray-700">
                  📱 QR
                </button>
              </div>
            </div>
          );
        })}
      </div>

      <div className="mt-6">
        <button className="px-4 py-2 bg-primary-600 text-white rounded-lg hover:bg-primary-700">
          ➕ Thêm bàn mới
        </button>
      </div>
    </div>
  );
}
