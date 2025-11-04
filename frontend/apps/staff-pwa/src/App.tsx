import { Link, Route, Routes } from "react-router-dom";

import Home from "./pages/Home.tsx";
import Orders from "./pages/Orders.tsx";
import Tasks from "./pages/Tasks.tsx";

function App() {
  return (
    <div className="pwa-shell">
      <header className="top-bar">
        <h1>Yakihouse Staff</h1>
        <nav>
          <Link to="/">Bàn</Link>
          <Link to="/orders">Order</Link>
          <Link to="/tasks">Nhiệm vụ</Link>
        </nav>
      </header>
      <main>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/orders" element={<Orders />} />
          <Route path="/tasks" element={<Tasks />} />
        </Routes>
      </main>
    </div>
  );
}

export default App;

