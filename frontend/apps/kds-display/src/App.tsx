import { useEffect } from 'react';
import { useKitchenStore } from './stores/kitchenStore';
import { useSignalR } from './hooks/useSignalR';

function App() {
  const { tickets, isLoading, fetchActiveOrders } = useKitchenStore();
  
  // Connect to SignalR
  useSignalR();

  // Fetch initial orders
  useEffect(() => {
    fetchActiveOrders();
  }, []);

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Pending':
        return 'bg-red-500 text-white';
      case 'InProgress':
        return 'bg-yellow-500 text-black';
      case 'Ready':
        return 'bg-green-500 text-white';
      case 'Delivered':
        return 'bg-gray-400 text-white';
      default:
        return 'bg-gray-200 text-gray-800';
    }
  };

  const getStatusText = (status: string) => {
    switch (status) {
      case 'Pending':
        return 'Mới vào';
      case 'InProgress':
        return 'Đang làm';
      case 'Ready':
        return 'Chờ ra món';
      case 'Delivered':
        return 'Đã ra món';
      default:
        return status;
    }
  };

  const getTimeColor = (elapsed: string) => {
    const [minutes] = elapsed.split(':').map(Number);
    if (minutes > 15) return 'text-red-600 font-bold';
    if (minutes > 10) return 'text-orange-500 font-semibold';
    return 'text-gray-700';
  };

  if (isLoading) {
    return (
      <div className="min-h-screen bg-gray-900 flex items-center justify-center">
        <div className="text-white text-2xl">Đang tải...</div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-900 p-6">
      {/* Header */}
      <header className="bg-gradient-to-r from-primary-600 to-primary-800 rounded-lg shadow-lg p-6 mb-6">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-3xl font-bold text-white mb-2">
              🔥 Yakihouse Kitchen Display
            </h1>
            <p className="text-primary-100">Theo dõi đơn hàng theo thời gian thực</p>
          </div>
          <div className="text-right">
            <div className="text-5xl font-bold text-white">{tickets.length}</div>
            <div className="text-primary-100">Đơn đang xử lý</div>
          </div>
        </div>
      </header>

      {/* Tickets Grid */}
      {tickets.length === 0 ? (
        <div className="text-center py-20">
          <div className="text-6xl mb-4">🍽️</div>
          <div className="text-gray-400 text-xl">Không có đơn hàng nào</div>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
          {tickets.map((ticket) => (
            <article
              key={ticket.id}
              className={`rounded-lg shadow-xl overflow-hidden border-4 ${
                ticket.status === 'Pending' ? 'border-red-500 animate-pulse' :
                ticket.status === 'InProgress' ? 'border-yellow-500' :
                ticket.status === 'Ready' ? 'border-green-500' :
                'border-gray-400'
              }`}
            >
              {/* Ticket Header */}
              <header className="bg-white p-4 border-b-2 border-gray-200">
                <div className="flex items-center justify-between mb-2">
                  <div className="flex items-center gap-3">
                    <span className="text-2xl font-bold text-gray-900">
                      Bàn {ticket.tableCode}
                    </span>
                    <span className={`px-3 py-1 rounded-full text-sm font-semibold ${getStatusColor(ticket.status)}`}>
                      {getStatusText(ticket.status)}
                    </span>
                  </div>
                </div>
                <div className="flex items-center justify-between">
                  <span className="text-sm text-gray-500">
                    #{ticket.id.slice(0, 8)}
                  </span>
                  <span className={`text-2xl font-mono ${getTimeColor(ticket.elapsed)}`}>
                    {ticket.elapsed}
                  </span>
                </div>
              </header>

              {/* Ticket Items */}
              <div className="bg-gray-50 p-4">
                <ul className="space-y-3">
                  {ticket.items.map((item) => (
                    <li key={item.id} className="bg-white rounded-lg p-3 shadow">
                      <div className="flex items-start gap-3">
                        <span className="text-2xl font-bold text-primary-600 min-w-[3rem]">
                          {item.quantity}×
                        </span>
                        <div className="flex-1">
                          <div className="font-semibold text-gray-900 text-lg">
                            {item.menuItemName}
                          </div>
                          {item.options.length > 0 && (
                            <div className="text-sm text-gray-600 mt-1">
                              {item.options.join(', ')}
                            </div>
                          )}
                          {item.note && (
                            <div className="text-sm text-orange-600 mt-1 font-medium">
                              📝 {item.note}
                            </div>
                          )}
                        </div>
                        <span className={`px-2 py-1 rounded text-xs font-semibold ${
                          item.status === 'Pending' ? 'bg-red-100 text-red-800' :
                          item.status === 'InProgress' ? 'bg-yellow-100 text-yellow-800' :
                          item.status === 'Ready' ? 'bg-green-100 text-green-800' :
                          'bg-gray-100 text-gray-800'
                        }`}>
                          {getStatusText(item.status)}
                        </span>
                      </div>
                    </li>
                  ))}
                </ul>
              </div>
            </article>
          ))}
        </div>
      )}

      {/* Connection Status Indicator */}
      <div className="fixed bottom-4 right-4 bg-green-500 text-white px-4 py-2 rounded-full shadow-lg flex items-center gap-2">
        <div className="w-3 h-3 bg-white rounded-full animate-pulse"></div>
        <span className="font-semibold">Đang kết nối</span>
      </div>
    </div>
  );
}

export default App;

