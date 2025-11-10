import { create } from 'zustand';
import axios from 'axios';

const API_URL = 'http://localhost:5194/api';

export interface StaffDto {
  id: string;
  fullName: string;
  email: string;
  phone: string;
  roleId: string;
  roleName: string;
  isActive: boolean;
  hireDate: string;
  createdAt: string;
}

export interface RoleDto {
  id: string;
  name: string;
  description?: string;
}

export interface CreateStaffDto {
  fullName: string;
  email: string;
  phone: string;
  roleId: string;
  hireDate: string;
}

export interface UpdateStaffDto {
  fullName: string;
  email: string;
  phone: string;
  roleId: string;
}

interface StaffState {
  staff: StaffDto[];
  roles: RoleDto[];
  isLoading: boolean;
  error: string | null;

  // Actions
  fetchStaff: (role?: string) => Promise<void>;
  fetchRoles: () => Promise<void>;
  createStaff: (data: CreateStaffDto) => Promise<any>;
  updateStaff: (id: string, data: UpdateStaffDto) => Promise<void>;
  deleteStaff: (id: string) => Promise<void>;
  toggleStaffStatus: (id: string) => Promise<void>;
  resetPassword: (id: string) => Promise<any>;
}

export const useStaffStore = create<StaffState>((set, get) => ({
  staff: [],
  roles: [],
  isLoading: false,
  error: null,

  fetchStaff: async (role?: string) => {
    set({ isLoading: true, error: null });
    try {
      const params = role ? { role } : {};
      const response = await axios.get<StaffDto[]>(`${API_URL}/Staff`, { params });
      set({ staff: response.data, isLoading: false });
    } catch (error: any) {
      set({ error: error.message, isLoading: false });
      console.error('Error fetching staff:', error);
    }
  },

  fetchRoles: async () => {
    try {
      const response = await axios.get<RoleDto[]>(`${API_URL}/Staff/roles`);
      set({ roles: response.data });
    } catch (error: any) {
      console.error('Error fetching roles:', error);
    }
  },

  createStaff: async (data: CreateStaffDto) => {
    try {
      const response = await axios.post(`${API_URL}/Staff`, data);
      await get().fetchStaff();
      return response.data; // Contains default password info
    } catch (error: any) {
      throw new Error(error.response?.data?.message || 'Không thể tạo nhân viên');
    }
  },

  updateStaff: async (id: string, data: UpdateStaffDto) => {
    try {
      await axios.put(`${API_URL}/Staff/${id}`, data);
      await get().fetchStaff();
    } catch (error: any) {
      throw new Error(error.response?.data?.message || 'Không thể cập nhật nhân viên');
    }
  },

  deleteStaff: async (id: string) => {
    try {
      await axios.delete(`${API_URL}/Staff/${id}`);
      await get().fetchStaff();
    } catch (error: any) {
      throw new Error(error.response?.data?.message || 'Không thể xóa nhân viên');
    }
  },

  toggleStaffStatus: async (id: string) => {
    try {
      await axios.patch(`${API_URL}/Staff/${id}/toggle-status`);
      await get().fetchStaff();
    } catch (error: any) {
      throw new Error(error.response?.data?.message || 'Không thể thay đổi trạng thái');
    }
  },

  resetPassword: async (id: string) => {
    try {
      const response = await axios.post(`${API_URL}/Staff/${id}/reset-password`);
      return response.data; // Contains new password
    } catch (error: any) {
      throw new Error(error.response?.data?.message || 'Không thể đặt lại mật khẩu');
    }
  },
}));
