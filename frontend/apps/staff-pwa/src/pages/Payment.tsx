import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useOrderStore } from '../stores/orderStore';
import axios from 'axios';
import type { OrderDto } from '../types/api';

type PaymentMethod = 'cash' | 'qr' | 'card';

export default function Payment() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { orders, fetchOrders } = useOrderStore();
  
  const [order, setOrder] = useState<OrderDto | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [paymentMethod, setPaymentMethod] = useState<PaymentMethod>('cash');
  const [cashReceived, setCashReceived] = useState<number>(0);
  const [isProcessing, setIsProcessing] = useState(false);

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

  const subtotal = calculateSubtotal();
  const tax = subtotal * 0.08; // 8% VAT
  const serviceCharge = subtotal * 0.05; // 5% service charge
  const total = subtotal + tax + serviceCharge;
  const change = cashReceived > total ? cashReceived - total : 0;

  const handlePayment = async () => {
    if (paymentMethod === 'cash' && cashReceived < total) {
      alert('Số tiền nhận không đủ!');
      return;
    }

    setIsProcessing(true);
    try {
      // Call API to create bill
      const response = await axios.post('http://localhost:5194/api/Bills', {
        orderId: order.id,
        paymentMethod: paymentMethod === 'cash' ? 'Cash' : paymentMethod === 'qr' ? 'QRCode' : 'Card',
        amountPaid: paymentMethod === 'cash' ? cashReceived : total,
        promotionIds: [] // TODO: Add promotion selection
      });

      if (response.status === 201) {
        alert('Thanh toán thành công!');
        navigate('/orders');
      }
    } catch (error: any) {
      console.error('Payment error:', error);
      alert(error.response?.data?.message || 'Có lỗi xảy ra khi thanh toán');
    } finally {
      setIsProcessing(false);
    }
  };

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <div className="bg-white shadow-sm">
        <div className="max-w-3xl mx-auto px-4 py-4">
          <button
            onClick={() => navigate(`/orders/${order.id}`)}
            className="text-gray-600 hover:text-gray-900 mb-4"
          >
            ← Quay lại
          </button>
          <h1 className="text-2xl font-bold text-gray-900">
            Thanh toán - Bàn {order.tableCode}
          </h1>
        </div>
      </div>

      <div className="max-w-3xl mx-auto px-4 py-6">
        {/* Order Summary */}
        <div className="bg-white rounded-lg shadow p-6 mb-4">
          <h2 className="text-lg font-semibold mb-4">Chi tiết hóa đơn</h2>
          
          <div className="space-y-2 mb-4">
            {order.items.map((item) => (
              <div key={item.id} className="flex justify-between text-sm">
                <span className="text-gray-700">
                  {item.quantity}x {item.menuItemName}
                </span>
                <span className="text-gray-900 font-medium">
                  {(item.unitPrice * item.quantity).toLocaleString('vi-VN')}đ
                </span>
              </div>
            ))}
          </div>

          <div className="border-t pt-4 space-y-2">
            <div className="flex justify-between">
              <span className="text-gray-600">Tạm tính:</span>
              <span className="font-medium">
                {subtotal.toLocaleString('vi-VN')}đ
              </span>
            </div>
            <div className="flex justify-between">
              <span className="text-gray-600">VAT (8%):</span>
              <span className="font-medium">
                {tax.toLocaleString('vi-VN')}đ
              </span>
            </div>
            <div className="flex justify-between">
              <span className="text-gray-600">Phí phục vụ (5%):</span>
              <span className="font-medium">
                {serviceCharge.toLocaleString('vi-VN')}đ
              </span>
            </div>
            <div className="flex justify-between text-xl font-bold border-t pt-2">
              <span>Tổng cộng:</span>
              <span className="text-primary-600">
                {total.toLocaleString('vi-VN')}đ
              </span>
            </div>
          </div>
        </div>

        {/* Payment Method */}
        <div className="bg-white rounded-lg shadow p-6 mb-4">
          <h2 className="text-lg font-semibold mb-4">Phương thức thanh toán</h2>
          
          <div className="grid grid-cols-3 gap-3">
            <button
              onClick={() => setPaymentMethod('cash')}
              className={`p-4 border-2 rounded-lg font-medium transition-all ${
                paymentMethod === 'cash'
                  ? 'border-primary-600 bg-primary-50 text-primary-700'
                  : 'border-gray-200 hover:border-gray-300'
              }`}
            >
              <div className="text-3xl mb-2">💵</div>
              <div>Tiền mặt</div>
            </button>

            <button
              onClick={() => setPaymentMethod('qr')}
              className={`p-4 border-2 rounded-lg font-medium transition-all ${
                paymentMethod === 'qr'
                  ? 'border-primary-600 bg-primary-50 text-primary-700'
                  : 'border-gray-200 hover:border-gray-300'
              }`}
            >
              <div className="text-3xl mb-2">📱</div>
              <div>QR Code</div>
            </button>

            <button
              onClick={() => setPaymentMethod('card')}
              className={`p-4 border-2 rounded-lg font-medium transition-all ${
                paymentMethod === 'card'
                  ? 'border-primary-600 bg-primary-50 text-primary-700'
                  : 'border-gray-200 hover:border-gray-300'
              }`}
            >
              <div className="text-3xl mb-2">💳</div>
              <div>Thẻ</div>
            </button>
          </div>
        </div>

        {/* Cash Payment Details */}
        {paymentMethod === 'cash' && (
          <div className="bg-white rounded-lg shadow p-6 mb-4">
            <h2 className="text-lg font-semibold mb-4">Thanh toán tiền mặt</h2>
            
            <div className="mb-4">
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Tiền khách đưa:
              </label>
              <input
                type="number"
                value={cashReceived || ''}
                onChange={(e) => setCashReceived(parseFloat(e.target.value) || 0)}
                className="w-full px-4 py-3 text-lg border-2 rounded-lg focus:border-primary-600 focus:outline-none"
                placeholder="Nhập số tiền..."
              />
            </div>

            <div className="grid grid-cols-3 gap-2 mb-4">
              {[50000, 100000, 200000, 500000].map((amount) => (
                <button
                  key={amount}
                  onClick={() => setCashReceived(amount)}
                  className="px-4 py-2 bg-gray-100 hover:bg-gray-200 rounded-lg font-medium"
                >
                  {amount.toLocaleString('vi-VN')}đ
                </button>
              ))}
            </div>

            {cashReceived > 0 && (
              <div className="p-4 bg-gray-50 rounded-lg">
                <div className="flex justify-between items-center">
                  <span className="text-gray-700">Tiền thừa:</span>
                  <span className={`text-xl font-bold ${
                    change >= 0 ? 'text-green-600' : 'text-red-600'
                  }`}>
                    {change.toLocaleString('vi-VN')}đ
                  </span>
                </div>
              </div>
            )}
          </div>
        )}

        {/* QR Code Payment */}
        {paymentMethod === 'qr' && (
          <div className="bg-white rounded-lg shadow p-6 mb-4 text-center">
            <h2 className="text-lg font-semibold mb-4">Quét mã QR để thanh toán</h2>
            <div className="w-64 h-64 bg-gray-100 mx-auto rounded-lg flex items-center justify-center mb-4">
              <div className="text-gray-400">
                <div className="text-6xl mb-2">📱</div>
                <div>QR Code sẽ hiển thị tại đây</div>
              </div>
            </div>
            <p className="text-sm text-gray-600">
              Quét mã QR bằng ứng dụng ngân hàng để thanh toán
            </p>
            <p className="text-lg font-semibold text-primary-600 mt-2">
              {total.toLocaleString('vi-VN')}đ
            </p>
          </div>
        )}

        {/* Card Payment */}
        {paymentMethod === 'card' && (
          <div className="bg-white rounded-lg shadow p-6 mb-4 text-center">
            <h2 className="text-lg font-semibold mb-4">Thanh toán bằng thẻ</h2>
            <div className="w-64 h-40 bg-gradient-to-br from-blue-500 to-purple-600 mx-auto rounded-lg flex items-center justify-center mb-4 text-white">
              <div className="text-4xl">💳</div>
            </div>
            <p className="text-sm text-gray-600 mb-4">
              Vui lòng đưa thẻ cho khách hàng quẹt hoặc chạm
            </p>
            <p className="text-lg font-semibold text-primary-600">
              {total.toLocaleString('vi-VN')}đ
            </p>
          </div>
        )}

        {/* Submit Button */}
        <button
          onClick={handlePayment}
          disabled={isProcessing || (paymentMethod === 'cash' && cashReceived < total)}
          className={`w-full py-4 rounded-lg font-semibold text-white text-lg ${
            isProcessing || (paymentMethod === 'cash' && cashReceived < total)
              ? 'bg-gray-400 cursor-not-allowed'
              : 'bg-green-600 hover:bg-green-700'
          }`}
        >
          {isProcessing ? 'Đang xử lý...' : 'Xác nhận thanh toán'}
        </button>
      </div>
    </div>
  );
}
