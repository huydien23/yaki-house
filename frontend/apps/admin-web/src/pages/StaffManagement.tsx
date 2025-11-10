import { useEffect, useState } from 'react';
import { useStaffStore, StaffDto, CreateStaffDto, UpdateStaffDto } from '../stores/staffStore';

export default function StaffManagement() {
  const { staff, roles, isLoading, fetchStaff, fetchRoles, toggleStaffStatus, deleteStaff, resetPassword } = useStaffStore();
  const [searchQuery, setSearchQuery] = useState('');
  const [selectedRole, setSelectedRole] = useState<string>('all');
  const [showModal, setShowModal] = useState(false);
  const [editingStaff, setEditingStaff] = useState<StaffDto | null>(null);

  useEffect(() => {
    fetchStaff();
    fetchRoles();
  }, []);

  const filteredStaff = staff.filter(s => {
    const matchRole = selectedRole === 'all' || s.roleName === selectedRole;
    const matchSearch = s.fullName.toLowerCase().includes(searchQuery.toLowerCase()) ||
                       s.email?.toLowerCase().includes(searchQuery.toLowerCase());
    return matchRole && matchSearch;
  });

  const handleToggleStatus = async (id: string) => {
    try {
      await toggleStaffStatus(id);
    } catch (error: any) {
      alert(error.message);
    }
  };

  const handleResetPassword = async (id: string, name: string) => {
    if (confirm(`Đặt lại mật khẩu cho nhân viên "${name}"?`)) {
      try {
        const result = await resetPassword(id);
        alert(`Mật khẩu mới: ${result.newPassword}\n${result.message}`);
      } catch (error: any) {
        alert(error.message);
      }
    }
  };

  const handleDelete = async (id: string, name: string) => {
    if (confirm(`Bạn có chắc muốn xóa nhân viên "${name}"?`)) {
      try {
        await deleteStaff(id);
        alert('Xóa nhân viên thành công!');
      } catch (error: any) {
        alert(error.message);
      }
    }
  };

  const getRoleBadge = (roleName: string) => {
    const styles: Record<string, string> = {
      'Admin': 'bg-purple-100 text-purple-700',
      'Manager': 'bg-blue-100 text-blue-700',
      'Staff': 'bg-green-100 text-green-700',
      'Kitchen': 'bg-orange-100 text-orange-700',
      'Waiter': 'bg-green-100 text-green-700'
    };
    const labels: Record<string, string> = {
      'Admin': '👑 Admin',
      'Manager': '📊 Quản Lý',
      'Staff': '🧑‍💼 Nhân Viên',
      'Kitchen': '👨‍🍳 Bếp',
      'Waiter': '🧑‍💼 Phục Vụ'
    };
    return { 
      style: styles[roleName] || 'bg-gray-100 text-gray-700', 
      label: labels[roleName] || roleName 
    };
  };

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-screen">
        <div className="text-xl">Đang tải...</div>
      </div>
    );
  }

  return (
    <div className="p-6">
      {/* Header */}
      <div className="mb-6">
        <h1 className="text-3xl font-bold text-gray-900 mb-2">Quản Lý Nhân Viên</h1>
        <p className="text-gray-600">Quản lý tài khoản và phân quyền nhân viên</p>
      </div>

      {/* Actions Bar */}
      <div className="bg-white rounded-lg shadow-sm p-4 mb-6">
        <div className="flex flex-wrap gap-4 items-center justify-between">
          {/* Search */}
          <div className="flex-1 min-w-[300px]">
            <input
              type="text"
              placeholder="🔍 Tìm kiếm nhân viên..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-primary-500"
            />
          </div>

          {/* Role Filter */}
          <select
            value={selectedRole}
            onChange={(e) => setSelectedRole(e.target.value)}
            className="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500"
          >
            <option value="all">Tất cả vai trò</option>
            <option value="Admin">👑 Admin</option>
            <option value="Manager">📊 Quản Lý</option>
            <option value="Staff">🧑‍💼 Phục Vụ</option>
            <option value="Kitchen">👨‍🍳 Bếp</option>
          </select>

          {/* Add Button */}
          <button
            onClick={() => {
              setEditingStaff(null);
              setShowModal(true);
            }}
            className="px-4 py-2 bg-primary-600 text-white rounded-lg hover:bg-primary-700 transition"
          >
            ➕ Thêm nhân viên
          </button>
        </div>
      </div>

      {/* Stats Cards */}
      <div className="grid grid-cols-1 md:grid-cols-5 gap-4 mb-6">
        <div className="bg-white rounded-lg shadow-sm p-4">
          <div className="text-sm text-gray-600 mb-1">Tổng số</div>
          <div className="text-2xl font-bold text-gray-900">{staff.length}</div>
        </div>
        <div className="bg-white rounded-lg shadow-sm p-4">
          <div className="text-sm text-gray-600 mb-1">Admin</div>
          <div className="text-2xl font-bold text-purple-600">
            {staff.filter(s => s.roleName === 'Admin').length}
          </div>
        </div>
        <div className="bg-white rounded-lg shadow-sm p-4">
          <div className="text-sm text-gray-600 mb-1">Quản lý</div>
          <div className="text-2xl font-bold text-blue-600">
            {staff.filter(s => s.roleName === 'Manager').length}
          </div>
        </div>
        <div className="bg-white rounded-lg shadow-sm p-4">
          <div className="text-sm text-gray-600 mb-1">Phục vụ</div>
          <div className="text-2xl font-bold text-green-600">
            {staff.filter(s => s.roleName === 'Staff').length}
          </div>
        </div>
        <div className="bg-white rounded-lg shadow-sm p-4">
          <div className="text-sm text-gray-600 mb-1">Bếp</div>
          <div className="text-2xl font-bold text-orange-600">
            {staff.filter(s => s.roleName === 'Kitchen').length}
          </div>
        </div>
      </div>

      {/* Staff Table */}
      {filteredStaff.length === 0 ? (
        <div className="bg-white rounded-lg shadow-sm p-12 text-center">
          <div className="text-6xl mb-4">👥</div>
          <div className="text-xl text-gray-600">Không tìm thấy nhân viên nào</div>
        </div>
      ) : (
        <div className="bg-white rounded-lg shadow-sm overflow-hidden">
          <table className="w-full">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-semibold text-gray-700 uppercase">Nhân viên</th>
                <th className="px-6 py-3 text-left text-xs font-semibold text-gray-700 uppercase">Username</th>
                <th className="px-6 py-3 text-left text-xs font-semibold text-gray-700 uppercase">Liên hệ</th>
                <th className="px-6 py-3 text-left text-xs font-semibold text-gray-700 uppercase">Vai trò</th>
                <th className="px-6 py-3 text-left text-xs font-semibold text-gray-700 uppercase">Trạng thái</th>
                <th className="px-6 py-3 text-left text-xs font-semibold text-gray-700 uppercase">Đăng nhập cuối</th>
                <th className="px-6 py-3 text-right text-xs font-semibold text-gray-700 uppercase">Thao tác</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-200">
              {filteredStaff.map(s => {
                const roleBadge = getRoleBadge(s.roleName);
                return (
                  <tr key={s.id} className="hover:bg-gray-50">
                    <td className="px-6 py-4">
                      <div className="flex items-center gap-3">
                        <div className="w-10 h-10 bg-primary-100 rounded-full flex items-center justify-center text-primary-600 font-bold">
                          {s.fullName.charAt(0)}
                        </div>
                        <div>
                          <div className="font-semibold text-gray-900">{s.fullName}</div>
                          <div className="text-sm text-gray-500">ID: {s.id.slice(0, 8)}</div>
                        </div>
                      </div>
                    </td>
                    <td className="px-6 py-4">
                      <div className="font-mono text-sm text-gray-700">-</div>
                    </td>
                    <td className="px-6 py-4">
                      <div className="text-sm text-gray-900">{s.email || '-'}</div>
                      <div className="text-sm text-gray-500">{s.phone || '-'}</div>
                    </td>
                    <td className="px-6 py-4">
                      <span className={`px-3 py-1 rounded-full text-xs font-semibold ${roleBadge.style}`}>
                        {roleBadge.label}
                      </span>
                    </td>
                    <td className="px-6 py-4">
                      <button
                        onClick={() => handleToggleStatus(s.id)}
                        className={`px-3 py-1 rounded-full text-xs font-semibold ${
                          s.isActive
                            ? 'bg-green-100 text-green-700 hover:bg-green-200'
                            : 'bg-red-100 text-red-700 hover:bg-red-200'
                        }`}
                      >
                        {s.isActive ? '✓ Hoạt động' : '✗ Khóa'}
                      </button>
                    </td>
                    <td className="px-6 py-4 text-sm text-gray-500">
                      {s.createdAt ? new Date(s.createdAt).toLocaleString('vi-VN') : '-'}
                    </td>
                    <td className="px-6 py-4">
                      <div className="flex gap-2 justify-end">
                        <button
                          onClick={() => {
                            setEditingStaff(s);
                            setShowModal(true);
                          }}
                          className="px-3 py-1 bg-blue-600 text-white text-sm rounded hover:bg-blue-700"
                        >
                          ✏️
                        </button>
                        <button
                          onClick={() => handleDelete(s.id, s.fullName)}
                          className="px-3 py-1 bg-red-600 text-white text-sm rounded hover:bg-red-700"
                        >
                          🗑️
                        </button>
                      </div>
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      )}

      {/* Modal */}
      {showModal && (
        <StaffModal
          staff={editingStaff}
          onClose={() => {
            setShowModal(false);
            setEditingStaff(null);
          }}
        />
      )}
    </div>
  );
}

// Staff Modal Component
function StaffModal({ staff, onClose }: { staff: StaffDto | null; onClose: () => void }) {
  const { roles, createStaff, updateStaff, resetPassword } = useStaffStore();
  const [formData, setFormData] = useState<CreateStaffDto | UpdateStaffDto>(
    staff
      ? {
          fullName: staff.fullName,
          email: staff.email || '',
          phone: staff.phone || '',
          roleId: staff.roleId
        }
      : {
          fullName: '',
          email: '',
          phone: '',
          roleId: '',
          hireDate: new Date().toISOString().split('T')[0]
        }
  );
  const [newPassword, setNewPassword] = useState('');
  const [showResetPassword, setShowResetPassword] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (staff) {
        await updateStaff(staff.id, formData as UpdateStaffDto);
        alert('Cập nhật nhân viên thành công!');
      } else {
        const result = await createStaff(formData as CreateStaffDto);
        alert(`Thêm nhân viên thành công!\nUsername: ${result.username}\nMật khẩu: ${result.defaultPassword}`);
      }
      onClose();
    } catch (error: any) {
      alert(error.message);
    }
  };

  const handleResetPasswordInModal = async () => {
    if (!staff || !newPassword || newPassword.length < 6) {
      alert('Mật khẩu phải có ít nhất 6 ký tự');
      return;
    }
    try {
      const result = await resetPassword(staff.id);
      alert(`Mật khẩu mới: ${result.newPassword}\n${result.message}`);
      setNewPassword('');
      setShowResetPassword(false);
    } catch (error: any) {
      alert(error.message);
    }
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div className="bg-white rounded-lg shadow-xl w-full max-w-2xl">
        {/* Header */}
        <div className="bg-primary-600 text-white p-4 flex items-center justify-between">
          <h2 className="text-xl font-bold">
            {staff ? '✏️ Sửa Nhân Viên' : '➕ Thêm Nhân Viên Mới'}
          </h2>
          <button onClick={onClose} className="text-2xl hover:text-gray-200">×</button>
        </div>

        {/* Form */}
        <form onSubmit={handleSubmit} className="p-6 space-y-4">
          <div className="grid grid-cols-2 gap-4">
            <div className="col-span-2">
              <label className="block text-sm font-semibold text-gray-700 mb-2">
                Họ và tên *
              </label>
              <input
                type="text"
                value={formData.fullName}
                onChange={(e) => setFormData({ ...formData, fullName: e.target.value })}
                required
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500"
                placeholder="VD: Nguyễn Văn A"
              />
            </div>

            <div>
              <label className="block text-sm font-semibold text-gray-700 mb-2">
                Email
              </label>
              <input
                type="email"
                value={formData.email}
                onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500"
                placeholder="example@yakihouse.com"
              />
            </div>

            <div>
              <label className="block text-sm font-semibold text-gray-700 mb-2">
                Số điện thoại
              </label>
              <input
                type="tel"
                value={formData.phone}
                onChange={(e) => setFormData({ ...formData, phone: e.target.value })}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500"
                placeholder="0901234567"
              />
            </div>

            <div>
              <label className="block text-sm font-semibold text-gray-700 mb-2">
                Vai trò *
              </label>
              <select
                value={formData.roleId}
                onChange={(e) => setFormData({ ...formData, roleId: e.target.value })}
                required
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500"
              >
                <option value="">Chọn vai trò...</option>
                {roles.map(role => (
                  <option key={role.id} value={role.id}>{role.name}</option>
                ))}
              </select>
            </div>

            {!staff && (
              <div>
                <label className="block text-sm font-semibold text-gray-700 mb-2">
                  Ngày vào làm *
                </label>
                <input
                  type="date"
                  value={(formData as CreateStaffDto).hireDate}
                  onChange={(e) => setFormData({ ...formData, hireDate: e.target.value })}
                  required
                  className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500"
                />
              </div>
            )}
          </div>

          {/* Reset Password Section */}
          {staff && (
            <div className="pt-4 border-t">
              {!showResetPassword ? (
                <button
                  type="button"
                  onClick={() => setShowResetPassword(true)}
                  className="text-blue-600 hover:text-blue-700 font-semibold"
                >
                  🔑 Đặt lại mật khẩu
                </button>
              ) : (
                <div className="flex gap-2">
                  <input
                    type="password"
                    value={newPassword}
                    onChange={(e) => setNewPassword(e.target.value)}
                    placeholder="Mật khẩu mới (tối thiểu 6 ký tự)"
                    className="flex-1 px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500"
                  />
                  <button
                    type="button"
                    onClick={handleResetPasswordInModal}
                    className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
                  >
                    Đặt lại
                  </button>
                  <button
                    type="button"
                    onClick={() => {
                      setShowResetPassword(false);
                      setNewPassword('');
                    }}
                    className="px-4 py-2 bg-gray-400 text-white rounded-lg hover:bg-gray-500"
                  >
                    Hủy
                  </button>
                </div>
              )}
            </div>
          )}

          {/* Actions */}
          <div className="flex gap-3 pt-4">
            <button
              type="submit"
              className="flex-1 px-4 py-2 bg-primary-600 text-white rounded-lg hover:bg-primary-700 font-semibold"
            >
              💾 {staff ? 'Cập nhật' : 'Thêm nhân viên'}
            </button>
            <button
              type="button"
              onClick={onClose}
              className="px-4 py-2 bg-gray-400 text-white rounded-lg hover:bg-gray-500"
            >
              Hủy
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
