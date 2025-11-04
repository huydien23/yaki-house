const demoTables = [
  { id: "T01", guests: 4, status: "Đang phục vụ" },
  { id: "T12", guests: 2, status: "Chờ món" },
  { id: "P03", guests: 10, status: "Chuẩn bị" }
];

function Home() {
  return (
    <section className="card">
      <h2>Bàn phụ trách</h2>
      <ul className="list">
        {demoTables.map((table) => (
          <li key={table.id}>
            <span className="primary">{table.id}</span>
            <span>{table.guests} khách</span>
            <span className="status">{table.status}</span>
          </li>
        ))}
      </ul>
    </section>
  );
}

export default Home;

