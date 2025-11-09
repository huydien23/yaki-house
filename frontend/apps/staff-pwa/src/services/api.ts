import axios from 'axios';
import type {
  OrderDto,
  MenuCategoryDto,
  MenuItemDto,
  TableDto,
  CreateOrderCommand,
  UpdateOrderStatusRequest,
} from '../types/api';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'https://localhost:7001/api';

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Orders API
export const ordersApi = {
  getActiveOrders: async (): Promise<OrderDto[]> => {
    const response = await apiClient.get<OrderDto[]>('/orders');
    return response.data;
  },

  getOrderById: async (id: string): Promise<OrderDto> => {
    const response = await apiClient.get<OrderDto>(`/orders/${id}`);
    return response.data;
  },

  createOrder: async (command: CreateOrderCommand): Promise<OrderDto> => {
    const response = await apiClient.post<OrderDto>('/orders', command);
    return response.data;
  },

  updateOrderStatus: async (
    id: string,
    request: UpdateOrderStatusRequest
  ): Promise<void> => {
    await apiClient.patch(`/orders/${id}/status`, request);
  },
};

// Menu API
export const menuApi = {
  getCategories: async (): Promise<MenuCategoryDto[]> => {
    const response = await apiClient.get<MenuCategoryDto[]>('/menu/categories');
    return response.data;
  },

  getMenuItems: async (categoryId?: string): Promise<MenuItemDto[]> => {
    const response = await apiClient.get<MenuItemDto[]>('/menu/items', {
      params: { categoryId },
    });
    return response.data;
  },

  getMenuItem: async (id: string): Promise<MenuItemDto> => {
    const response = await apiClient.get<MenuItemDto>(`/menu/items/${id}`);
    return response.data;
  },
};

// Tables API
export const tablesApi = {
  getTables: async (zone?: string): Promise<TableDto[]> => {
    const response = await apiClient.get<TableDto[]>('/tables', {
      params: { zone },
    });
    return response.data;
  },

  getTable: async (id: string): Promise<TableDto> => {
    const response = await apiClient.get<TableDto>(`/tables/${id}`);
    return response.data;
  },

  getZones: async (): Promise<string[]> => {
    const response = await apiClient.get<string[]>('/tables/zones');
    return response.data;
  },
};

export default apiClient;

