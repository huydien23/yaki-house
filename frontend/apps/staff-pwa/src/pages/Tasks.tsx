const taskList = [
  { id: 1, label: "Bổ sung nước sốt khu A", due: "Ngay" },
  { id: 2, label: "Dọn bàn T05", due: "Trong 5 phút" },
  { id: 3, label: "Xác nhận thanh toán bàn P02", due: "Trước giờ đóng ca" }
];

function Tasks() {
  return (
    <section className="card">
      <h2>Nhiệm vụ ca làm</h2>
      <ul className="list">
        {taskList.map((task) => (
          <li key={task.id}>
            <span className="primary">{task.label}</span>
            <span>{task.due}</span>
          </li>
        ))}
      </ul>
    </section>
  );
}

export default Tasks;

