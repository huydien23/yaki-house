import { useEffect } from 'react';
import { Link } from 'react-router-dom';
import { useOrderStore } from '../stores/orderStore';

export default function Orders() {
  const { orders, fetchOrders, isLoading } = useOrderStore();

  useEffect(() => {
    fetchOrders();
  }, []);

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Draft':
        return 'bg-gray-200 text-gray-800';
      case 'Confirmed':
        return 'bg-blue-200 text-blue-800';
      case 'InProgress':
        return 'bg-yellow-200 text-yellow-800';
      case 'Ready':
        return 'bg-green-200 text-green-800';
      case 'Completed':
        return 'bg-green-500 text-white';
      case 'Cancelled':
        return 'bg-red-200 text-red-800';
      default:
        return 'bg-gray-200 text-gray-800';
    }
  };

  if (isLoading) {
    return (
      <div className="min-h-screen bg-gray-50 p-6 flex items-center justify-center">
        <div className="text-xl">Đang tải...</div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50 p-6">
      <div className="max-w-7xl mx-auto">
        <div className="flex justify-between items-center mb-6">
          <h1 className="text-3xl font-bold text-gray-900">Đơn hàng</h1>
          <Link
            to="/"
            className="px-4 py-2 bg-primary-600 text-white rounded-lg hover:bg-primary-700"
          >
            Quay lại
          </Link>
        </div>

        {orders.length === 0 ? (
          <div className="bg-white rounded-lg shadow p-8 text-center">
            <p className="text-gray-500 text-lg">Không có đơn hàng nào</p>
          </div>
        ) : (
          <div className="space-y-4">
            {orders.map((order) => (
              <Link
                key={order.id}
                to={`/orders/${order.id}`}
                className="block bg-white rounded-lg shadow hover:shadow-lg transition-shadow p-6"
              >
                <div className="flex justify-between items-start mb-4">
                  <div>
                    <h3 className="text-xl font-bold text-gray-900">
                      Bàn {order.tableCode}
                    </h3>
                    <p className="text-sm text-gray-600">
                      Nhân viên: {order.staffName}
                    </p>
                    <p className="text-sm text-gray-600">
                      Số khách: {order.guestCount}
                    </p>
                  </div>
                  <span
                    className={`px-3 py-1 rounded-full text-sm font-medium ${getStatusColor(
                      order.status
                    )}`}
                  >
                    {order.status}
                  </span>
                </div>

                <div className="border-t pt-4">
                  <p className="text-sm text-gray-600 mb-2">
                    {order.items.length} món
                  </p>
                  <div className="space-y-1">
                    {order.items.slice(0, 3).map((item) => (
                      <div
                        key={item.id}
                        className="flex justify-between text-sm"
                      >
                        <span className="text-gray-700">
                          {item.quantity}x {item.menuItemName}
                        </span>
                        <span className="text-gray-600">
                          {(item.unitPrice * item.quantity).toLocaleString('vi-VN')}đ
                        </span>
                      </div>
                    ))}
                    {order.items.length > 3 && (
                      <p className="text-sm text-gray-500">
                        +{order.items.length - 3} món khác...
                      </p>
                    )}
                  </div>
                </div>

                <div className="mt-4 text-sm text-gray-500">
                  {new Date(order.createdAt).toLocaleString('vi-VN')}
                </div>
              </Link>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}
