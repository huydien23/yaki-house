const tickets = [
  {
    id: "KT-1201",
    table: "T01",
    status: "Đang làm",
    elapsed: "04:22",
    items: [
      { name: "Bò Mỹ", qty: 2 },
      { name: "Tôm nướng", qty: 1 }
    ]
  },
  {
    id: "KT-1202",
    table: "T07",
    status: "Chờ ra món",
    elapsed: "01:15",
    items: [{ name: "Buffet Premium", qty: 4 }]
  },
  {
    id: "KT-1203",
    table: "P02",
    status: "Mới vào",
    elapsed: "00:32",
    items: [
      { name: "Gà Teriyaki", qty: 3 },
      { name: "Hàu nướng", qty: 3 }
    ]
  }
];

function App() {
  return (
    <div className="kds-shell">
      <header>
        <h1>Yakihouse Kitchen Display</h1>
        <p>Theo dõi ticket theo thời gian thực.</p>
      </header>
      <main>
        {tickets.map((ticket) => (
          <article key={ticket.id} className="ticket">
            <header>
              <div>
                <span className="ticket-id">{ticket.id}</span>
                <span className="ticket-table">Bàn {ticket.table}</span>
              </div>
              <div className="ticket-meta">
                <span className="ticket-status">{ticket.status}</span>
                <span className="ticket-time">{ticket.elapsed}</span>
              </div>
            </header>
            <ul>
              {ticket.items.map((item) => (
                <li key={item.name}>
                  <span className="qty">{item.qty}×</span>
                  <span className="name">{item.name}</span>
                </li>
              ))}
            </ul>
          </article>
        ))}
      </main>
    </div>
  );
}

export default App;

