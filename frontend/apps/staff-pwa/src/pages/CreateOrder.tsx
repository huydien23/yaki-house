import { useEffect, useMemo, useState } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { useMenuStore } from '../stores/menuStore';
import { useOrderStore } from '../stores/orderStore';
import { useTableStore } from '../stores/tableStore';
import { useAuthStore } from '../stores/authStore';

interface CartItem {
  menuItemId: string;
  name: string;
  basePrice: number;
  quantity: number;
  note: string;
  options: Array<{
    menuOptionId: string;
    optionName: string;
    extraPrice: number;
    quantity: number;
  }>;
}

export default function CreateOrder() {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const tableId = searchParams.get('tableId');

  const { categories, menuItems, fetchCategories, fetchMenuItems } = useMenuStore();
  const { createOrder } = useOrderStore();
  const { tables, fetchTables } = useTableStore();

  const [selectedCategory, setSelectedCategory] = useState<string | null>(null);
  const [searchQuery, setSearchQuery] = useState('');
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(12);
  const [cart, setCart] = useState<CartItem[]>([]);
  const [guestCount, setGuestCount] = useState(2);
  const [adultCount, setAdultCount] = useState(2);
  const [childCount, setChildCount] = useState(0);
  const [notes, setNotes] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);

  const table = tables.find(t => t.id === tableId);

  useEffect(() => {
    fetchCategories();
    fetchMenuItems();
    if (!tables.length) {
      fetchTables();
    }
  }, []);

  useEffect(() => {
    setCurrentPage(1);
  }, [selectedCategory, searchQuery, pageSize]);

  const filteredItems = useMemo(() => {
    let items = selectedCategory
      ? menuItems.filter(item => item.categoryId === selectedCategory)
      : menuItems;

    if (searchQuery.trim()) {
      const keyword = searchQuery.trim().toLowerCase();
      items = items.filter(
        item =>
          item.name.toLowerCase().includes(keyword) ||
          item.description?.toLowerCase().includes(keyword)
      );
    }

    return items;
  }, [menuItems, selectedCategory, searchQuery]);

  const totalPages = useMemo(
    () => Math.max(1, Math.ceil(filteredItems.length / pageSize)),
    [filteredItems.length, pageSize]
  );

  const pagedItems = useMemo(() => {
    const start = (currentPage - 1) * pageSize;
    return filteredItems.slice(start, start + pageSize);
  }, [filteredItems, currentPage, pageSize]);

  const addToCart = (item: typeof menuItems[0]) => {
    const existingItem = cart.find(c => c.menuItemId === item.id);
    
    if (existingItem) {
      setCart(cart.map(c => 
        c.menuItemId === item.id 
          ? { ...c, quantity: c.quantity + 1 }
          : c
      ));
    } else {
      setCart([...cart, {
        menuItemId: item.id,
        name: item.name,
        basePrice: item.basePrice,
        quantity: 1,
        note: '',
        options: []
      }]);
    }
  };

  const removeFromCart = (menuItemId: string) => {
    setCart(cart.filter(c => c.menuItemId !== menuItemId));
  };

  const updateQuantity = (menuItemId: string, newQuantity: number) => {
    if (newQuantity === 0) {
      removeFromCart(menuItemId);
    } else {
      setCart(cart.map(c =>
        c.menuItemId === menuItemId
          ? { ...c, quantity: newQuantity }
          : c
      ));
    }
  };

  const updateNote = (menuItemId: string, note: string) => {
    setCart(cart.map(c =>
      c.menuItemId === menuItemId
        ? { ...c, note }
        : c
    ));
  };

  const calculateTotal = () => {
    return cart.reduce((sum, item) => {
      const itemTotal = item.basePrice * item.quantity;
      const optionsTotal = item.options.reduce(
        (optSum, opt) => optSum + (opt.extraPrice * opt.quantity),
        0
      );
      return sum + itemTotal + optionsTotal;
    }, 0);
  };

  const handleSubmit = async () => {
    if (!tableId || cart.length === 0) {
      alert('Vui lòng chọn bàn và thêm món vào giỏ hàng');
      return;
    }

    const { user } = useAuthStore.getState();
    if (!user) {
      alert('Bạn cần đăng nhập để tạo đơn hàng');
      return;
    }

    setIsSubmitting(true);
    try {
      await createOrder({
        tableId,
        staffId: user.id,
        guestCount,
        adultCount,
        childCount,
        childHeights: null,
        buffetType: 'Nuong',
        hasDessertBuffet: false,
        notes,
        items: cart.map(item => ({
          menuItemId: item.menuItemId,
          quantity: item.quantity,
          note: item.note,
          options: item.options
        }))
      });

      alert('Tạo đơn hàng thành công!');
      navigate('/orders');
    } catch (error) {
      console.error('Failed to create order:', error);
      alert('Có lỗi xảy ra khi tạo đơn hàng');
    } finally {
      setIsSubmitting(false);
    }
  };

  if (!tableId) {
    return (
      <div className="min-h-screen bg-gray-50 p-6">
        <div className="max-w-7xl mx-auto">
          <div className="bg-red-50 border border-red-200 rounded-lg p-4">
            <h2 className="text-lg font-semibold text-red-800">
              Chưa chọn bàn
            </h2>
            <p className="text-red-600 mt-2">
              Vui lòng quay lại trang chủ và chọn bàn
            </p>
            <button
              onClick={() => navigate('/')}
              className="mt-4 px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700"
            >
              Quay lại trang chủ
            </button>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <div className="bg-white shadow-sm sticky top-0 z-10">
        <div className="max-w-7xl mx-auto px-4 py-4">
          <div className="flex items-center justify-between">
            <div>
              <h1 className="text-2xl font-bold text-gray-900">
                Tạo đơn hàng - {table?.code || 'Loading...'}
              </h1>
              <p className="text-sm text-gray-600">
                Khu vực: {table?.zone} • Sức chứa: {table?.capacity} người
              </p>
            </div>
            <button
              onClick={() => navigate('/')}
              className="px-4 py-2 text-gray-700 hover:bg-gray-100 rounded-lg"
            >
              ← Quay lại
            </button>
          </div>
        </div>
      </div>

      <div className="max-w-7xl mx-auto px-4 py-6">
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
          {/* Menu Section */}
          <div className="lg:col-span-2">
            {/* Guest Count */}
            <div className="bg-white rounded-lg shadow p-4 mb-4">
              <h3 className="font-semibold mb-3">Số khách</h3>
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm text-gray-600 mb-1">
                    Người lớn
                  </label>
                  <input
                    type="number"
                    min="0"
                    value={adultCount}
                    onChange={(e) => {
                      const val = parseInt(e.target.value) || 0;
                      setAdultCount(val);
                      setGuestCount(val + childCount);
                    }}
                    className="w-full px-3 py-2 border rounded-lg"
                  />
                </div>
                <div>
                  <label className="block text-sm text-gray-600 mb-1">
                    Trẻ em
                  </label>
                  <input
                    type="number"
                    min="0"
                    value={childCount}
                    onChange={(e) => {
                      const val = parseInt(e.target.value) || 0;
                      setChildCount(val);
                      setGuestCount(adultCount + val);
                    }}
                    className="w-full px-3 py-2 border rounded-lg"
                  />
                </div>
              </div>
            </div>

            {/* Categories */}
            <div className="bg-white rounded-lg shadow p-4 mb-4">
              <h3 className="font-semibold mb-3">Danh mục</h3>
              <div className="flex flex-wrap gap-2">
                <button
                  onClick={() => setSelectedCategory(null)}
                  className={`px-4 py-2 rounded-lg font-medium whitespace-nowrap ${
                    selectedCategory === null
                      ? 'bg-primary-600 text-white'
                      : 'bg-gray-100 text-gray-700 hover:bg-gray-200'
                  }`}
                >
                  Tất cả
                </button>
                {categories.map((cat) => (
                  <button
                    key={cat.id}
                    onClick={() => setSelectedCategory(cat.id)}
                    className={`px-4 py-2 rounded-lg font-medium whitespace-nowrap ${
                      selectedCategory === cat.id
                        ? 'bg-primary-600 text-white'
                        : 'bg-gray-100 text-gray-700 hover:bg-gray-200'
                    }`}
                  >
                    {cat.name}
                  </button>
                ))}
              </div>

              <div className="mt-4 grid grid-cols-1 md:grid-cols-2 gap-3">
                <div>
                  <label className="block text-sm text-gray-600 mb-1">
                    Tìm kiếm món ăn
                  </label>
                  <input
                    type="text"
                    value={searchQuery}
                    onChange={(e) => setSearchQuery(e.target.value)}
                    placeholder="Nhập tên món..."
                    className="w-full px-3 py-2 border rounded-lg"
                  />
                </div>
                <div>
                  <label className="block text-sm text-gray-600 mb-1">
                    Số món mỗi trang
                  </label>
                  <select
                    value={pageSize}
                    onChange={(e) => setPageSize(Number(e.target.value))}
                    className="w-full px-3 py-2 border rounded-lg"
                  >
                    {[9, 12, 18, 24].map((size) => (
                      <option key={size} value={size}>
                        {size} món
                      </option>
                    ))}
                  </select>
                </div>
              </div>
            </div>

            {/* Menu Items */}
            <div className="bg-white rounded-lg shadow p-4">
              <h3 className="font-semibold mb-3">Món ăn</h3>
              {pagedItems.length === 0 ? (
                <p className="text-center text-gray-500 py-8">
                  Không có món ăn nào
                </p>
              ) : (
                <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
                  {pagedItems.map((item) => (
                    <button
                      key={item.id}
                      onClick={() => addToCart(item)}
                      className="text-left p-3 border rounded-lg hover:border-primary-500 hover:shadow-md transition-all"
                    >
                      <div className="font-medium text-gray-900">
                        {item.name}
                      </div>
                      {item.description && (
                        <div className="text-xs text-gray-600 mt-1 line-clamp-2">
                          {item.description}
                        </div>
                      )}
                      <div className="text-primary-600 font-semibold mt-2">
                        {item.basePrice.toLocaleString('vi-VN')}đ
                      </div>
                      <div className={`text-xs mt-1 ${
                        item.status === 'Available' 
                          ? 'text-green-600' 
                          : 'text-red-600'
                      }`}>
                        {item.status === 'Available' ? 'Còn món' : 'Hết món'}
                      </div>
                    </button>
                  ))}
                </div>
              )}

              {/* Pagination */}
              {filteredItems.length > pageSize && (
                <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 mt-6">
                  <p className="text-sm text-gray-600">
                    Hiển thị {pagedItems.length} / {filteredItems.length} món
                  </p>
                  <div className="flex items-center gap-2">
                    <button
                      onClick={() => setCurrentPage((prev) => Math.max(1, prev - 1))}
                      disabled={currentPage === 1}
                      className="px-3 py-2 rounded border border-gray-300 text-sm disabled:opacity-50 disabled:cursor-not-allowed"
                    >
                      ← Trang trước
                    </button>
                    <span className="text-sm text-gray-700">
                      Trang {currentPage} / {totalPages}
                    </span>
                    <button
                      onClick={() => setCurrentPage((prev) => Math.min(totalPages, prev + 1))}
                      disabled={currentPage === totalPages}
                      className="px-3 py-2 rounded border border-gray-300 text-sm disabled:opacity-50 disabled:cursor-not-allowed"
                    >
                      Trang sau →
                    </button>
                  </div>
                </div>
              )}
            </div>
          </div>

          {/* Cart Section */}
          <div className="lg:col-span-1">
            <div className="bg-white rounded-lg shadow p-4 sticky top-24">
              <h3 className="font-semibold text-lg mb-3">
                Giỏ hàng ({cart.length})
              </h3>

              {cart.length === 0 ? (
                <p className="text-center text-gray-500 py-8">
                  Chưa có món nào
                </p>
              ) : (
                <>
                  <div className="space-y-3 mb-4 max-h-96 overflow-y-auto">
                    {cart.map((item) => (
                      <div
                        key={item.menuItemId}
                        className="border rounded-lg p-3"
                      >
                        <div className="flex justify-between items-start mb-2">
                          <div className="flex-1">
                            <div className="font-medium">{item.name}</div>
                            <div className="text-sm text-gray-600">
                              {item.basePrice.toLocaleString('vi-VN')}đ
                            </div>
                          </div>
                          <button
                            onClick={() => removeFromCart(item.menuItemId)}
                            className="text-red-600 hover:text-red-700"
                          >
                            ✕
                          </button>
                        </div>

                        <div className="flex items-center gap-2 mb-2">
                          <button
                            onClick={() =>
                              updateQuantity(item.menuItemId, item.quantity - 1)
                            }
                            className="w-8 h-8 bg-gray-100 rounded hover:bg-gray-200"
                          >
                            −
                          </button>
                          <span className="w-12 text-center font-medium">
                            {item.quantity}
                          </span>
                          <button
                            onClick={() =>
                              updateQuantity(item.menuItemId, item.quantity + 1)
                            }
                            className="w-8 h-8 bg-gray-100 rounded hover:bg-gray-200"
                          >
                            +
                          </button>
                        </div>

                        <input
                          type="text"
                          placeholder="Ghi chú..."
                          value={item.note}
                          onChange={(e) =>
                            updateNote(item.menuItemId, e.target.value)
                          }
                          className="w-full px-2 py-1 text-sm border rounded"
                        />

                        <div className="text-right text-sm font-semibold text-primary-600 mt-2">
                          {(item.basePrice * item.quantity).toLocaleString('vi-VN')}đ
                        </div>
                      </div>
                    ))}
                  </div>

                  {/* Notes */}
                  <div className="mb-4">
                    <label className="block text-sm text-gray-600 mb-1">
                      Ghi chú chung
                    </label>
                    <textarea
                      value={notes}
                      onChange={(e) => setNotes(e.target.value)}
                      className="w-full px-3 py-2 border rounded-lg"
                      rows={2}
                      placeholder="Ghi chú cho đơn hàng..."
                    />
                  </div>

                  {/* Total */}
                  <div className="border-t pt-3 mb-4">
                    <div className="flex justify-between items-center text-lg font-bold">
                      <span>Tổng cộng:</span>
                      <span className="text-primary-600">
                        {calculateTotal().toLocaleString('vi-VN')}đ
                      </span>
                    </div>
                  </div>

                  {/* Submit */}
                  <button
                    onClick={handleSubmit}
                    disabled={isSubmitting}
                    className={`w-full py-3 rounded-lg font-semibold text-white ${
                      isSubmitting
                        ? 'bg-gray-400 cursor-not-allowed'
                        : 'bg-primary-600 hover:bg-primary-700'
                    }`}
                  >
                    {isSubmitting ? 'Đang tạo...' : 'Tạo đơn hàng'}
                  </button>
                </>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
