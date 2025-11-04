const palette = [
  { tone: 50, className: "bg-primary-50 text-primary-900" },
  { tone: 100, className: "bg-primary-100 text-primary-900" },
  { tone: 200, className: "bg-primary-200 text-primary-900" },
  { tone: 300, className: "bg-primary-300 text-primary-900" },
  { tone: 400, className: "bg-primary-400 text-white" },
  { tone: 500, className: "bg-primary-500 text-white" },
  { tone: 600, className: "bg-primary-600 text-white" },
  { tone: 700, className: "bg-primary-700 text-white" },
  { tone: 800, className: "bg-primary-800 text-primary-100" },
  { tone: 900, className: "bg-primary-900 text-primary-100" },
  { tone: 950, className: "bg-primary-950 text-primary-100" }
];

function App() {
  return (
    <div className="mx-auto flex max-w-5xl flex-col gap-10 px-6 py-16">
      <header className="space-y-3">
        <span className="inline-flex items-center gap-2 rounded-full bg-primary-100 px-3 py-1 text-sm font-semibold text-primary-700">
          Yakihouse Platform
        </span>
        <h1 className="font-display text-4xl font-semibold text-primary-900 md:text-5xl">
          Admin Console
        </h1>
        <p className="max-w-3xl text-lg text-primary-700">
          Khởi tạo giao diện quản trị để theo dõi doanh thu, cấu hình menu, phân ca nhân viên
          và giám sát bếp theo thời gian thực với nền màu Cerulean chuẩn thương hiệu.
        </p>
      </header>

      <main className="grid gap-8 md:grid-cols-2">
        <section className="card space-y-3 border border-primary-100">
          <h2 className="text-xl font-semibold text-primary-800">Lộ trình UI</h2>
          <ul className="list-inside list-disc space-y-2 text-primary-700">
            <li>Tạo layout tổng thể với sidebar điều hướng và header trạng thái.</li>
            <li>Kết nối bảng dữ liệu bàn, ca, order qua API .NET Core.</li>
            <li>Thiết lập trang dashboard với biểu đồ doanh thu & KPI phục vụ.</li>
          </ul>
        </section>

        <section className="card space-y-4 bg-gradient-to-br from-primary-100 via-white to-primary-50">
          <h2 className="text-xl font-semibold text-primary-800">Màu Cerulean</h2>
          <p className="text-primary-700">
            Tailwind đã được cấu hình với bảng màu Cerulean (50 → 950) như hình. Dùng các lớp
            <code className="ml-1 rounded bg-primary-100 px-1 text-sm text-primary-800">bg-primary-600</code>,
            <code className="ml-1 rounded bg-primary-100 px-1 text-sm text-primary-800">text-primary-900</code>
            để thống nhất thương hiệu trong toàn dự án.
          </p>
          <div className="flex flex-wrap gap-2">
            {palette.map((swatch) => (
              <span
                key={swatch.tone}
                className={`flex h-10 min-w-[4.5rem] items-center justify-center rounded-xl px-3 text-sm font-semibold shadow ${swatch.className}`}
              >
                {swatch.tone}
              </span>
            ))}
          </div>
        </section>
      </main>
    </div>
  );
}

export default App;

