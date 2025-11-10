import { create } from 'zustand';
import type { MenuCategoryDto, MenuItemDto } from '../types/api';
import { menuApi } from '../services/api';

interface MenuState {
  categories: MenuCategoryDto[];
  menuItems: MenuItemDto[];
  selectedCategory: string | null;
  isLoading: boolean;
  error: string | null;
  
  // Actions
  fetchCategories: () => Promise<void>;
  fetchMenuItems: (categoryId?: string) => Promise<void>;
  setSelectedCategory: (categoryId: string | null) => void;
}

export const useMenuStore = create<MenuState>((set) => ({
  categories: [],
  menuItems: [],
  selectedCategory: null,
  isLoading: false,
  error: null,

  fetchCategories: async () => {
    set({ isLoading: true, error: null });
    try {
      const categories = await menuApi.getCategories();
      set({ categories, isLoading: false });
    } catch (error) {
      set({ error: 'Failed to fetch categories', isLoading: false });
      console.error('Error fetching categories:', error);
    }
  },

  fetchMenuItems: async (categoryId?: string) => {
    set({ isLoading: true, error: null });
    try {
      const menuItems = await menuApi.getMenuItems(categoryId);
      set({ menuItems, isLoading: false });
    } catch (error) {
      set({ error: 'Failed to fetch menu items', isLoading: false });
      console.error('Error fetching menu items:', error);
    }
  },

  setSelectedCategory: (categoryId: string | null) => {
    set({ selectedCategory: categoryId });
  },
}));

