import { useEffect, useMemo, useState } from 'react';
import { Link } from 'react-router-dom';
import { useTableStore } from '../stores/tableStore';
import { useOrderStore } from '../stores/orderStore';

export default function Home() {
  const { tables, fetchTables, fetchZones, selectedZone, setSelectedZone, zones } = useTableStore();
  const { orders, fetchOrders } = useOrderStore();
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(12);

  useEffect(() => {
    fetchTables();
    fetchZones();
    fetchOrders();
  }, []);

  useEffect(() => {
    if (selectedZone) {
      fetchTables(selectedZone);
    } else {
      fetchTables();
    }
  }, [selectedZone]);

  useEffect(() => {
    setCurrentPage(1);
  }, [selectedZone, pageSize]);

  const totalPages = useMemo(() => Math.max(1, Math.ceil(tables.length / pageSize)), [tables.length, pageSize]);

  useEffect(() => {
    if (currentPage > totalPages) {
      setCurrentPage(totalPages);
    }
  }, [currentPage, totalPages]);

  const pagedTables = useMemo(() => {
    const start = (currentPage - 1) * pageSize;
    return tables.slice(start, start + pageSize);
  }, [tables, currentPage, pageSize]);

  const getTableStatusColor = (status: string) => {
    switch (status) {
      case 'Available':
        return 'bg-green-500';
      case 'Occupied':
        return 'bg-red-500';
      case 'Reserved':
        return 'bg-yellow-500';
      default:
        return 'bg-gray-500';
    }
  };

  return (
    <div className="min-h-screen bg-gray-50 p-6">
      <div className="max-w-7xl mx-auto">
        <div className="mb-6">
          <h1 className="text-3xl font-bold text-gray-900 mb-4">Yakihouse - Staff App</h1>
          
          {/* Zone Filter */}
          <div className="flex flex-wrap gap-2 mb-4">
            <button
              onClick={() => setSelectedZone(null)}
              className={`px-4 py-2 rounded-lg font-medium whitespace-nowrap ${
                selectedZone === null
                  ? 'bg-primary-600 text-white'
                  : 'bg-white text-gray-700 border border-gray-300'
              }`}
            >
              Tất cả
            </button>
            {zones.map((zone) => (
              <button
                key={zone}
                onClick={() => setSelectedZone(zone)}
                className={`px-4 py-2 rounded-lg font-medium whitespace-nowrap ${
                  selectedZone === zone
                    ? 'bg-primary-600 text-white'
                    : 'bg-white text-gray-700 border border-gray-300'
                }`}
              >
                {zone}
              </button>
            ))}
          </div>
        </div>

        {/* Active Orders Summary */}
        <div className="bg-white rounded-lg shadow p-4 mb-6">
          <h2 className="text-xl font-semibold mb-2">Đơn hàng đang hoạt động</h2>
          <p className="text-3xl font-bold text-primary-600">{orders.length}</p>
        </div>

        {/* Tables Grid */}
        <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
          {pagedTables.map((table) => (
            <Link
              key={table.id}
              to={`/orders/new?tableId=${table.id}`}
              className="bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow"
            >
              <div className="flex items-center justify-between mb-2">
                <h3 className="text-xl font-bold">{table.code}</h3>
                <div className={`w-3 h-3 rounded-full ${getTableStatusColor(table.status)}`} />
              </div>
              {table.zone && (
                <p className="text-sm text-gray-600 mb-1">{table.zone}</p>
              )}
              <p className="text-sm text-gray-500">Sức chứa: {table.capacity}</p>
              <p className="text-sm font-medium mt-2 text-gray-700">{table.status}</p>
            </Link>
          ))}
          {pagedTables.length === 0 && (
            <div className="col-span-full text-center text-gray-500 py-12 bg-white rounded-lg shadow">
              Không có bàn nào trong khu vực này.
            </div>
          )}
        </div>

        {/* Pagination */}
        <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 mt-6">
          <div className="flex items-center gap-2">
            <span className="text-sm text-gray-600">Hiển thị mỗi trang</span>
            <select
              value={pageSize}
              onChange={(e) => setPageSize(Number(e.target.value))}
              className="border border-gray-300 rounded px-3 py-1 text-sm"
            >
              {[8, 12, 16, 20].map((size) => (
                <option key={size} value={size}>
                  {size} bàn
                </option>
              ))}
            </select>
          </div>
          <div className="flex items-center gap-2">
            <button
              onClick={() => setCurrentPage((prev) => Math.max(1, prev - 1))}
              disabled={currentPage === 1}
              className="px-3 py-2 rounded border border-gray-300 text-sm disabled:opacity-50 disabled:cursor-not-allowed"
            >
              ← Trang trước
            </button>
            <span className="text-sm text-gray-700">
              Trang {currentPage} / {totalPages}
            </span>
            <button
              onClick={() => setCurrentPage((prev) => Math.min(totalPages, prev + 1))}
              disabled={currentPage === totalPages}
              className="px-3 py-2 rounded border border-gray-300 text-sm disabled:opacity-50 disabled:cursor-not-allowed"
            >
              Trang sau →
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}
