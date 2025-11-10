import { useEffect, useState } from 'react';
import { useMenuStore, MenuItemDto, MenuCategoryDto } from '../stores/menuStore';

export default function MenuManagement() {
  const { categories, items, isLoading, fetchCategories, fetchItems, toggleItemStatus, deleteItem } = useMenuStore();
  const [selectedCategory, setSelectedCategory] = useState<string>('all');
  const [searchQuery, setSearchQuery] = useState('');
  const [showCategoryModal, setShowCategoryModal] = useState(false);
  const [showItemModal, setShowItemModal] = useState(false);
  const [editingItem, setEditingItem] = useState<MenuItemDto | null>(null);

  useEffect(() => {
    fetchCategories();
    fetchItems();
  }, []);

  const filteredItems = items.filter(item => {
    const matchCategory = selectedCategory === 'all' || item.categoryId === selectedCategory;
    const matchSearch = item.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
                       item.description?.toLowerCase().includes(searchQuery.toLowerCase());
    return matchCategory && matchSearch;
  });

  const handleDeleteItem = async (id: string, name: string) => {
    if (confirm(`Bạn có chắc muốn xóa món "${name}"?`)) {
      try {
        await deleteItem(id);
        alert('Xóa món thành công!');
      } catch (error: any) {
        alert(error.message);
      }
    }
  };

  const handleToggleStatus = async (id: string, currentStatus: boolean) => {
    try {
      await toggleItemStatus(id, !currentStatus);
    } catch (error: any) {
      alert(error.message);
    }
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
        <h1 className="text-3xl font-bold text-gray-900 mb-2">Quản Lý Menu</h1>
        <p className="text-gray-600">Quản lý danh mục và món ăn của nhà hàng</p>
      </div>

      {/* Actions Bar */}
      <div className="bg-white rounded-lg shadow-sm p-4 mb-6">
        <div className="flex flex-wrap gap-4 items-center justify-between">
          {/* Search */}
          <div className="flex-1 min-w-[300px]">
            <input
              type="text"
              placeholder="🔍 Tìm kiếm món ăn..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-primary-500"
            />
          </div>

          {/* Category Filter */}
          <select
            value={selectedCategory}
            onChange={(e) => setSelectedCategory(e.target.value)}
            className="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500"
          >
            <option value="all">Tất cả danh mục</option>
            {categories.map(cat => (
              <option key={cat.id} value={cat.id}>{cat.name}</option>
            ))}
          </select>

          {/* Action Buttons */}
          <div className="flex gap-2">
            <button
              onClick={() => setShowCategoryModal(true)}
              className="px-4 py-2 bg-gray-600 text-white rounded-lg hover:bg-gray-700 transition"
            >
              📁 Danh mục
            </button>
            <button
              onClick={() => {
                setEditingItem(null);
                setShowItemModal(true);
              }}
              className="px-4 py-2 bg-primary-600 text-white rounded-lg hover:bg-primary-700 transition"
            >
              ➕ Thêm món
            </button>
          </div>
        </div>
      </div>

      {/* Stats Cards */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-4 mb-6">
        <div className="bg-white rounded-lg shadow-sm p-4">
          <div className="text-sm text-gray-600 mb-1">Tổng số món</div>
          <div className="text-2xl font-bold text-gray-900">{items.length}</div>
        </div>
        <div className="bg-white rounded-lg shadow-sm p-4">
          <div className="text-sm text-gray-600 mb-1">Đang bán</div>
          <div className="text-2xl font-bold text-green-600">
            {items.filter(i => i.isAvailable).length}
          </div>
        </div>
        <div className="bg-white rounded-lg shadow-sm p-4">
          <div className="text-sm text-gray-600 mb-1">Hết hàng</div>
          <div className="text-2xl font-bold text-red-600">
            {items.filter(i => !i.isAvailable).length}
          </div>
        </div>
        <div className="bg-white rounded-lg shadow-sm p-4">
          <div className="text-sm text-gray-600 mb-1">Danh mục</div>
          <div className="text-2xl font-bold text-primary-600">{categories.length}</div>
        </div>
      </div>

      {/* Items Grid */}
      {filteredItems.length === 0 ? (
        <div className="bg-white rounded-lg shadow-sm p-12 text-center">
          <div className="text-6xl mb-4">🍽️</div>
          <div className="text-xl text-gray-600">Không tìm thấy món ăn nào</div>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
          {filteredItems.map(item => (
            <div key={item.id} className="bg-white rounded-lg shadow-sm overflow-hidden hover:shadow-md transition">
              {/* Image */}
              <div className="h-48 bg-gradient-to-br from-primary-100 to-primary-200 flex items-center justify-center">
                {item.imageUrl ? (
                  <img src={item.imageUrl} alt={item.name} className="w-full h-full object-cover" />
                ) : (
                  <div className="text-6xl">🍽️</div>
                )}
              </div>

              {/* Content */}
              <div className="p-4">
                {/* Category Badge */}
                <div className="text-xs text-primary-600 font-semibold mb-2">
                  {item.categoryName}
                </div>

                {/* Name */}
                <h3 className="text-lg font-bold text-gray-900 mb-2 line-clamp-2">
                  {item.name}
                </h3>

                {/* Description */}
                {item.description && (
                  <p className="text-sm text-gray-600 mb-3 line-clamp-2">
                    {item.description}
                  </p>
                )}

                {/* Price & Time */}
                <div className="flex items-center justify-between mb-3">
                  <div className="text-lg font-bold text-primary-600">
                    {item.basePrice.toLocaleString('vi-VN')}đ
                  </div>
                  <div className="text-sm text-gray-500">
                    ⏱️ {item.preparationTime} phút
                  </div>
                </div>

                {/* Status */}
                <div className="flex items-center gap-2 mb-3">
                  <button
                    onClick={() => handleToggleStatus(item.id, item.isAvailable)}
                    className={`flex-1 px-3 py-2 rounded-lg font-semibold transition ${
                      item.isAvailable
                        ? 'bg-green-100 text-green-700 hover:bg-green-200'
                        : 'bg-red-100 text-red-700 hover:bg-red-200'
                    }`}
                  >
                    {item.isAvailable ? '✓ Đang bán' : '✗ Hết hàng'}
                  </button>
                </div>

                {/* Actions */}
                <div className="flex gap-2">
                  <button
                    onClick={() => {
                      setEditingItem(item);
                      setShowItemModal(true);
                    }}
                    className="flex-1 px-3 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition"
                  >
                    ✏️ Sửa
                  </button>
                  <button
                    onClick={() => handleDeleteItem(item.id, item.name)}
                    className="px-3 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 transition"
                  >
                    🗑️
                  </button>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}

      {/* Category Modal */}
      {showCategoryModal && (
        <CategoryModal
          categories={categories}
          onClose={() => setShowCategoryModal(false)}
        />
      )}

      {/* Item Modal */}
      {showItemModal && (
        <ItemModal
          item={editingItem}
          categories={categories}
          onClose={() => {
            setShowItemModal(false);
            setEditingItem(null);
          }}
        />
      )}
    </div>
  );
}

// Category Modal Component
function CategoryModal({ categories, onClose }: { categories: MenuCategoryDto[]; onClose: () => void }) {
  const { createCategory, updateCategory, deleteCategory } = useMenuStore();
  const [editingId, setEditingId] = useState<string | null>(null);
  const [name, setName] = useState('');
  const [displayOrder, setDisplayOrder] = useState(0);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (editingId) {
        await updateCategory(editingId, name, displayOrder);
      } else {
        await createCategory(name, displayOrder);
      }
      setName('');
      setDisplayOrder(0);
      setEditingId(null);
    } catch (error: any) {
      alert(error.message);
    }
  };

  const handleEdit = (cat: MenuCategoryDto) => {
    setEditingId(cat.id);
    setName(cat.name);
    setDisplayOrder(cat.displayOrder);
  };

  const handleDelete = async (id: string, name: string) => {
    if (confirm(`Xóa danh mục "${name}"?`)) {
      try {
        await deleteCategory(id);
      } catch (error: any) {
        alert(error.message);
      }
    }
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div className="bg-white rounded-lg shadow-xl w-full max-w-2xl max-h-[80vh] overflow-hidden">
        {/* Header */}
        <div className="bg-primary-600 text-white p-4 flex items-center justify-between">
          <h2 className="text-xl font-bold">📁 Quản Lý Danh Mục</h2>
          <button onClick={onClose} className="text-2xl hover:text-gray-200">×</button>
        </div>

        <div className="p-6 overflow-y-auto max-h-[calc(80vh-120px)]">
          {/* Form */}
          <form onSubmit={handleSubmit} className="mb-6 p-4 bg-gray-50 rounded-lg">
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="block text-sm font-semibold text-gray-700 mb-2">
                  Tên danh mục *
                </label>
                <input
                  type="text"
                  value={name}
                  onChange={(e) => setName(e.target.value)}
                  required
                  className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500"
                  placeholder="VD: Buffet Nướng"
                />
              </div>
              <div>
                <label className="block text-sm font-semibold text-gray-700 mb-2">
                  Thứ tự hiển thị
                </label>
                <input
                  type="number"
                  value={displayOrder}
                  onChange={(e) => setDisplayOrder(Number(e.target.value))}
                  className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500"
                />
              </div>
            </div>
            <div className="flex gap-2 mt-4">
              <button
                type="submit"
                className="px-4 py-2 bg-primary-600 text-white rounded-lg hover:bg-primary-700"
              >
                {editingId ? '💾 Cập nhật' : '➕ Thêm'}
              </button>
              {editingId && (
                <button
                  type="button"
                  onClick={() => {
                    setEditingId(null);
                    setName('');
                    setDisplayOrder(0);
                  }}
                  className="px-4 py-2 bg-gray-400 text-white rounded-lg hover:bg-gray-500"
                >
                  Hủy
                </button>
              )}
            </div>
          </form>

          {/* List */}
          <div className="space-y-2">
            {categories.map(cat => (
              <div key={cat.id} className="flex items-center justify-between p-3 bg-white border rounded-lg hover:shadow-sm">
                <div className="flex items-center gap-3">
                  <div className="text-sm font-semibold text-gray-500">#{cat.displayOrder}</div>
                  <div className="font-semibold text-gray-900">{cat.name}</div>
                </div>
                <div className="flex gap-2">
                  <button
                    onClick={() => handleEdit(cat)}
                    className="px-3 py-1 bg-blue-600 text-white text-sm rounded hover:bg-blue-700"
                  >
                    ✏️
                  </button>
                  <button
                    onClick={() => handleDelete(cat.id, cat.name)}
                    className="px-3 py-1 bg-red-600 text-white text-sm rounded hover:bg-red-700"
                  >
                    🗑️
                  </button>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
}

// Item Modal Component
function ItemModal({ item, categories, onClose }: { item: MenuItemDto | null; categories: MenuCategoryDto[]; onClose: () => void }) {
  const { createItem, updateItem } = useMenuStore();
  const [formData, setFormData] = useState({
    name: item?.name || '',
    categoryId: item?.categoryId || categories[0]?.id || '',
    description: item?.description || '',
    basePrice: item?.basePrice || 0,
    preparationTime: item?.preparationTime || 15,
    isAvailable: item?.isAvailable ?? true
  });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (item) {
        await updateItem(item.id, formData);
      } else {
        await createItem(formData);
      }
      alert('Lưu món thành công!');
      onClose();
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
            {item ? '✏️ Sửa Món' : '➕ Thêm Món Mới'}
          </h2>
          <button onClick={onClose} className="text-2xl hover:text-gray-200">×</button>
        </div>

        {/* Form */}
        <form onSubmit={handleSubmit} className="p-6 space-y-4">
          <div className="grid grid-cols-2 gap-4">
            <div className="col-span-2">
              <label className="block text-sm font-semibold text-gray-700 mb-2">
                Tên món *
              </label>
              <input
                type="text"
                value={formData.name}
                onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                required
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500"
                placeholder="VD: Bò Mỹ Nướng"
              />
            </div>

            <div>
              <label className="block text-sm font-semibold text-gray-700 mb-2">
                Danh mục *
              </label>
              <select
                value={formData.categoryId}
                onChange={(e) => setFormData({ ...formData, categoryId: e.target.value })}
                required
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500"
              >
                {categories.map(cat => (
                  <option key={cat.id} value={cat.id}>{cat.name}</option>
                ))}
              </select>
            </div>

            <div>
              <label className="block text-sm font-semibold text-gray-700 mb-2">
                Giá (VNĐ) *
              </label>
              <input
                type="number"
                value={formData.basePrice}
                onChange={(e) => setFormData({ ...formData, basePrice: Number(e.target.value) })}
                required
                min="0"
                step="1000"
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500"
              />
            </div>

            <div>
              <label className="block text-sm font-semibold text-gray-700 mb-2">
                Thời gian chế biến (phút)
              </label>
              <input
                type="number"
                value={formData.preparationTime}
                onChange={(e) => setFormData({ ...formData, preparationTime: Number(e.target.value) })}
                min="1"
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500"
              />
            </div>

            <div>
              <label className="block text-sm font-semibold text-gray-700 mb-2">
                Trạng thái
              </label>
              <select
                value={formData.isAvailable ? 'available' : 'unavailable'}
                onChange={(e) => setFormData({ ...formData, isAvailable: e.target.value === 'available' })}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500"
              >
                <option value="available">Đang bán</option>
                <option value="unavailable">Hết hàng</option>
              </select>
            </div>

            <div className="col-span-2">
              <label className="block text-sm font-semibold text-gray-700 mb-2">
                Mô tả
              </label>
              <textarea
                value={formData.description}
                onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                rows={3}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500"
                placeholder="Mô tả chi tiết về món ăn..."
              />
            </div>
          </div>

          {/* Actions */}
          <div className="flex gap-3 pt-4">
            <button
              type="submit"
              className="flex-1 px-4 py-2 bg-primary-600 text-white rounded-lg hover:bg-primary-700 font-semibold"
            >
              💾 {item ? 'Cập nhật' : 'Thêm món'}
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
