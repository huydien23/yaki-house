import { Link, Route, Routes, Navigate } from "react-router-dom";
import { useAuthStore } from "./stores/authStore";

import Home from "./pages/Home.tsx";
import Orders from "./pages/Orders.tsx";
import Tasks from "./pages/Tasks.tsx";
import CreateOrder from "./pages/CreateOrder.tsx";
import OrderDetail from "./pages/OrderDetail.tsx";
import Payment from "./pages/Payment.tsx";
import Login from "./pages/Login.tsx";
import ProtectedRoute from "./components/ProtectedRoute.tsx";

function App() {
  const { isAuthenticated, user, logout } = useAuthStore();

  return (
    <div className="pwa-shell">
      <Routes>
        <Route path="/login" element={<Login />} />
        
        <Route
          path="/*"
          element={
            <ProtectedRoute>
              <div className="flex flex-col h-screen">
                <header className="bg-primary-600 text-white shadow-md">
                  <div className="px-4 py-3 flex items-center justify-between">
                    <h1 className="text-xl font-bold">Yakihouse Staff</h1>
                    <div className="flex items-center gap-4">
                      <span className="text-sm">{user?.fullName}</span>
                      <button
                        onClick={logout}
                        className="px-3 py-1 bg-white/20 hover:bg-white/30 rounded text-sm"
                      >
                        Đăng xuất
                      </button>
                    </div>
                  </div>
                  <nav className="px-4 pb-2 flex gap-4">
                    <Link to="/" className="text-white/80 hover:text-white font-medium">
                      Bàn
                    </Link>
                    <Link to="/orders" className="text-white/80 hover:text-white font-medium">
                      Order
                    </Link>
                    <Link to="/tasks" className="text-white/80 hover:text-white font-medium">
                      Nhiệm vụ
                    </Link>
                  </nav>
                </header>
                <main className="flex-1 overflow-auto">
                  <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/orders" element={<Orders />} />
                    <Route path="/orders/new" element={<CreateOrder />} />
                    <Route path="/orders/:id" element={<OrderDetail />} />
                    <Route path="/orders/:id/payment" element={<Payment />} />
                    <Route path="/tasks" element={<Tasks />} />
                  </Routes>
                </main>
              </div>
            </ProtectedRoute>
          }
        />
      </Routes>
    </div>
  );
}

export default App;

