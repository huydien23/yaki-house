const demoOrders = [
  {
    id: "ORD-2301",
    table: "T01",
    items: [
      { name: "Buffet Premium", qty: 4 },
      { name: "Thịt bò Wagyu", qty: 1 }
    ],
    status: "Đã gửi bếp"
  },
  {
    id: "ORD-2302",
    table: "T12",
    items: [{ name: "Buffet tiêu chuẩn", qty: 2 }],
    status: "Chờ xác nhận"
  }
];

function Orders() {
  return (
    <section className="card">
      <h2>Order gần nhất</h2>
      <div className="stack">
        {demoOrders.map((order) => (
          <article key={order.id} className="order-card">
            <header>
              <span className="primary">{order.id}</span>
              <span>Bàn {order.table}</span>
            </header>
            <ul>
              {order.items.map((item) => (
                <li key={item.name}>
                  {item.qty} × {item.name}
                </li>
              ))}
            </ul>
            <footer>{order.status}</footer>
          </article>
        ))}
      </div>
    </section>
  );
}

export default Orders;

