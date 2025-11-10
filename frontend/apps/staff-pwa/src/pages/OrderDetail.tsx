import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useOrderStore } from '../stores/orderStore';
import { useOrderSignalR } from '../hooks/useOrderSignalR';
import type { OrderDto } from '../types/api';

export default function OrderDetail() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { orders, fetchOrders } = useOrderStore();
  const [order, setOrder] = useState<OrderDto | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  // Connect to SignalR for real-time updates
  useOrderSignalR(true);

  useEffect(() => {
    const loadOrder = async () => {
      if (!orders.length) {
        await fetchOrders();
      }
      const foundOrder = orders.find(o => o.id === id);
      setOrder(foundOrder || null);
      setIsLoading(false);
    };
    loadOrder();
  }, [id, orders]);

  // Update order when orders list changes
  useEffect(() => {
    const foundOrder = orders.find(o => o.id === id);
    if (foundOrder) {
      setOrder(foundOrder);
    }
  }, [orders, id]);

  if (isLoading) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="text-xl">Đang tải...</div>
      </div>
    );
  }

  if (!order) {
    return (
      <div className="min-h-screen bg-gray-50 p-6">
        <div className="max-w-3xl mx-auto">
          <div className="bg-white rounded-lg shadow p-8 text-center">
            <h2 className="text-2xl font-bold text-gray-900 mb-4">
              Không tìm thấy đơn hàng
            </h2>
            <button
              onClick={() => navigate('/orders')}
              className="px-6 py-2 bg-primary-600 text-white rounded-lg hover:bg-primary-700"
            >
              Quay lại danh sách
            </button>
          </div>
        </div>
      </div>
    );
  }

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Draft':
        return 'bg-gray-200 text-gray-800';
      case 'Confirmed':
      case 'Submitted':
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

  const calculateSubtotal = () => {
    return order.items.reduce((sum, item) => {
      const itemTotal = item.unitPrice * item.quantity;
      const optionsTotal = item.options.reduce(
        (optSum, opt) => optSum + (opt.extraPrice * opt.quantity),
        0
      );
      return sum + itemTotal + optionsTotal;
    }, 0);
  };

  const statusText: Record<string, string> = {
    'Draft': 'Nháp',
    'Confirmed': 'Đã xác nhận',
    'Submitted': 'Đã gửi bếp',
    'InProgress': 'Đang chế biến',
    'Ready': 'Sẵn sàng',
    'Completed': 'Hoàn thành',
    'Cancelled': 'Đã hủy'
  };

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <div className="bg-white shadow-sm">
        <div className="max-w-3xl mx-auto px-4 py-4">
          <div className="flex items-center justify-between mb-4">
            <button
              onClick={() => navigate('/orders')}
              className="text-gray-600 hover:text-gray-900"
            >
              ← Quay lại
            </button>
            <span className={`px-3 py-1 rounded-full text-sm font-medium ${getStatusColor(order.status)}`}>
              {statusText[order.status] || order.status}
            </span>
          </div>
          
          <h1 className="text-2xl font-bold text-gray-900">
            Chi tiết đơn hàng
          </h1>
        </div>
      </div>

      <div className="max-w-3xl mx-auto px-4 py-6">
        {/* Order Info */}
        <div className="bg-white rounded-lg shadow p-6 mb-4">
          <h2 className="text-lg font-semibold mb-4">Thông tin đơn hàng</h2>
          <div className="grid grid-cols-2 gap-4">
            <div>
              <div className="text-sm text-gray-600">Bàn</div>
              <div className="font-medium">{order.tableCode}</div>
            </div>
            <div>
              <div className="text-sm text-gray-600">Nhân viên</div>
              <div className="font-medium">{order.staffName}</div>
            </div>
            <div>
              <div className="text-sm text-gray-600">Số khách</div>
              <div className="font-medium">{order.guestCount} người</div>
            </div>
            <div>
              <div className="text-sm text-gray-600">Thời gian</div>
              <div className="font-medium">
                {new Date(order.createdAt).toLocaleString('vi-VN')}
              </div>
            </div>
          </div>
          {order.notes && (
            <div className="mt-4 p-3 bg-gray-50 rounded">
              <div className="text-sm text-gray-600 mb-1">Ghi chú:</div>
              <div className="text-gray-900">{order.notes}</div>
            </div>
          )}
        </div>

        {/* Order Items */}
        <div className="bg-white rounded-lg shadow p-6 mb-4">
          <h2 className="text-lg font-semibold mb-4">Món ăn ({order.items.length})</h2>
          <div className="space-y-4">
            {order.items.map((item) => (
              <div key={item.id} className="border-b pb-4 last:border-b-0 last:pb-0">
                <div className="flex justify-between items-start mb-2">
                  <div className="flex-1">
                    <div className="font-medium">{item.menuItemName}</div>
                    <div className="text-sm text-gray-600">
                      {item.unitPrice.toLocaleString('vi-VN')}đ x {item.quantity}
                    </div>
                  </div>
                  <div className="text-right">
                    <div className="font-semibold text-primary-600">
                      {(item.unitPrice * item.quantity).toLocaleString('vi-VN')}đ
                    </div>
                    <div className={`text-xs mt-1 ${
                      item.status === 'Ready' 
                        ? 'text-green-600' 
                        : item.status === 'InProgress'
                        ? 'text-yellow-600'
                        : 'text-gray-600'
                    }`}>
                      {statusText[item.status] || item.status}
                    </div>
                  </div>
                </div>
                
                {item.options.length > 0 && (
                  <div className="ml-4 text-sm text-gray-600">
                    {item.options.map((opt, idx) => (
                      <div key={idx}>
                        + {opt.optionName} ({opt.quantity}) 
                        {opt.extraPrice > 0 && ` +${opt.extraPrice.toLocaleString('vi-VN')}đ`}
                      </div>
                    ))}
                  </div>
                )}
                
                {item.note && (
                  <div className="ml-4 mt-2 text-sm text-gray-600 italic">
                    Ghi chú: {item.note}
                  </div>
                )}
              </div>
            ))}
          </div>
        </div>

        {/* Total */}
        <div className="bg-white rounded-lg shadow p-6 mb-4">
          <div className="flex justify-between items-center text-xl font-bold">
            <span>Tổng cộng:</span>
            <span className="text-primary-600">
              {calculateSubtotal().toLocaleString('vi-VN')}đ
            </span>
          </div>
        </div>

        {/* Actions */}
        {(order.status === 'Submitted' || order.status === 'Ready') && (
          <div className="bg-white rounded-lg shadow p-6">
            <div className="grid grid-cols-2 gap-4">
              {order.status === 'Ready' && (
                <button
                  onClick={() => navigate(`/orders/${order.id}/payment`)}
                  className="py-3 bg-green-600 text-white rounded-lg font-semibold hover:bg-green-700"
                >
                  Thanh toán
                </button>
              )}
              <button
                onClick={() => alert('Chức năng đang phát triển')}
                className="py-3 bg-gray-200 text-gray-700 rounded-lg font-semibold hover:bg-gray-300"
              >
                Thêm món
              </button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
