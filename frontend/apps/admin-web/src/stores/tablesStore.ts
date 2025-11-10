import { create } from 'zustand';
import axios from 'axios';

const API_URL = 'http://localhost:5194/api';

export interface TableDto {
  id: string;
  code: string;
  zone?: string;
  capacity: number;
  status: 'Available' | 'Occupied' | 'Reserved' | 'Maintenance';
}

export interface CreateTableDto {
  code: string;
  capacity: number;
  zone?: string;
}

export interface UpdateTableDto {
  code: string;
  capacity: number;
  zone?: string;
}

interface TablesState {
  tables: TableDto[];
  zones: string[];
  isLoading: boolean;
  error: string | null;

  // Actions
  fetchTables: (zone?: string) => Promise<void>;
  fetchZones: () => Promise<void>;
  createTable: (data: CreateTableDto) => Promise<void>;
  updateTable: (id: string, data: UpdateTableDto) => Promise<void>;
  deleteTable: (id: string) => Promise<void>;
  updateTableStatus: (id: string, status: TableDto['status']) => Promise<void>;
}

export const useTablesStore = create<TablesState>((set, get) => ({
  tables: [],
  zones: [],
  isLoading: false,
  error: null,

  fetchTables: async (zone?: string) => {
    set({ isLoading: true, error: null });
    try {
      const params = zone ? { zone } : {};
      const response = await axios.get<TableDto[]>(`${API_URL}/Tables`, { params });
      set({ tables: response.data, isLoading: false });
    } catch (error: any) {
      set({ error: error.message, isLoading: false });
      console.error('Error fetching tables:', error);
    }
  },

  fetchZones: async () => {
    try {
      const response = await axios.get<string[]>(`${API_URL}/Tables/zones`);
      set({ zones: response.data });
    } catch (error: any) {
      console.error('Error fetching zones:', error);
    }
  },

  createTable: async (data: CreateTableDto) => {
    try {
      await axios.post(`${API_URL}/Tables`, data);
      await get().fetchTables();
    } catch (error: any) {
      throw new Error(error.response?.data?.message || 'Không thể tạo bàn');
    }
  },

  updateTable: async (id: string, data: UpdateTableDto) => {
    try {
      await axios.put(`${API_URL}/Tables/${id}`, data);
      await get().fetchTables();
    } catch (error: any) {
      throw new Error(error.response?.data?.message || 'Không thể cập nhật bàn');
    }
  },

  deleteTable: async (id: string) => {
    try {
      await axios.delete(`${API_URL}/Tables/${id}`);
      await get().fetchTables();
    } catch (error: any) {
      throw new Error(error.response?.data?.message || 'Không thể xóa bàn');
    }
  },

  updateTableStatus: async (id: string, status: TableDto['status']) => {
    try {
      await axios.patch(`${API_URL}/Tables/${id}/status`, { status });
      await get().fetchTables();
    } catch (error: any) {
      throw new Error(error.response?.data?.message || 'Không thể cập nhật trạng thái');
    }
  },
}));
