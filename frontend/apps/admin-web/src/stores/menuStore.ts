import { create } from 'zustand';
import axios from 'axios';

const API_URL = 'http://localhost:5194/api';

export interface MenuCategoryDto {
  id: string;
  name: string;
  displayOrder: number;
}

export interface MenuItemDto {
  id: string;
  categoryId: string;
  categoryName: string;
  name: string;
  description?: string;
  basePrice: number;
  status: string;
  isAvailable: boolean;
  preparationTime: number;
  imageUrl?: string;
  optionGroups: MenuOptionGroupDto[];
}

export interface MenuOptionGroupDto {
  id: string;
  name: string;
  isRequired: boolean;
  minSelection: number;
  maxSelection: number;
  displayOrder: number;
  options: MenuOptionDto[];
}

export interface MenuOptionDto {
  id: string;
  name: string;
  extraPrice: number;
  isAvailable: boolean;
  displayOrder: number;
}

interface MenuState {
  categories: MenuCategoryDto[];
  items: MenuItemDto[];
  isLoading: boolean;
  error: string | null;

  // Actions
  fetchCategories: () => Promise<void>;
  fetchItems: () => Promise<void>;
  createCategory: (name: string, displayOrder: number) => Promise<void>;
  updateCategory: (id: string, name: string, displayOrder: number) => Promise<void>;
  deleteCategory: (id: string) => Promise<void>;
  createItem: (item: Partial<MenuItemDto>) => Promise<void>;
  updateItem: (id: string, item: Partial<MenuItemDto>) => Promise<void>;
  deleteItem: (id: string) => Promise<void>;
  toggleItemStatus: (id: string, isAvailable: boolean) => Promise<void>;
}

export const useMenuStore = create<MenuState>((set, get) => ({
  categories: [],
  items: [],
  isLoading: false,
  error: null,

  fetchCategories: async () => {
    set({ isLoading: true, error: null });
    try {
      const response = await axios.get<MenuCategoryDto[]>(`${API_URL}/Menu/categories`);
      set({ categories: response.data, isLoading: false });
    } catch (error: any) {
      set({ error: error.message, isLoading: false });
    }
  },

  fetchItems: async () => {
    set({ isLoading: true, error: null });
    try {
      const response = await axios.get<MenuItemDto[]>(`${API_URL}/Menu/items`);
      set({ items: response.data, isLoading: false });
    } catch (error: any) {
      set({ error: error.message, isLoading: false });
    }
  },

  createCategory: async (name: string, displayOrder: number) => {
    try {
      const response = await axios.post(`${API_URL}/Menu/categories`, { name, displayOrder });
      await get().fetchCategories();
      return response.data;
    } catch (error: any) {
      throw new Error(error.response?.data?.message || 'Không thể tạo danh mục');
    }
  },

  updateCategory: async (id: string, name: string, displayOrder: number) => {
    try {
      await axios.put(`${API_URL}/Menu/categories/${id}`, { name, displayOrder });
      await get().fetchCategories();
    } catch (error: any) {
      throw new Error(error.response?.data?.message || 'Không thể cập nhật danh mục');
    }
  },

  deleteCategory: async (id: string) => {
    try {
      await axios.delete(`${API_URL}/Menu/categories/${id}`);
      await get().fetchCategories();
    } catch (error: any) {
      throw new Error(error.response?.data?.message || 'Không thể xóa danh mục');
    }
  },

  createItem: async (item: Partial<MenuItemDto>) => {
    try {
      const response = await axios.post(`${API_URL}/Menu/items`, item);
      await get().fetchItems();
      return response.data;
    } catch (error: any) {
      throw new Error(error.response?.data?.message || 'Không thể tạo món ăn');
    }
  },

  updateItem: async (id: string, item: Partial<MenuItemDto>) => {
    try {
      await axios.put(`${API_URL}/Menu/items/${id}`, item);
      await get().fetchItems();
    } catch (error: any) {
      throw new Error(error.response?.data?.message || 'Không thể cập nhật món ăn');
    }
  },

  deleteItem: async (id: string) => {
    try {
      await axios.delete(`${API_URL}/Menu/items/${id}`);
      await get().fetchItems();
    } catch (error: any) {
      throw new Error(error.response?.data?.message || 'Không thể xóa món ăn');
    }
  },

  toggleItemStatus: async (id: string, isAvailable: boolean) => {
    try {
      await axios.patch(`${API_URL}/Menu/items/${id}/status`, { isAvailable });
      set(state => ({
        items: state.items.map(item =>
          item.id === id ? { ...item, isAvailable } : item
        )
      }));
    } catch (error: any) {
      throw new Error(error.response?.data?.message || 'Không thể thay đổi trạng thái');
    }
  }
}));
