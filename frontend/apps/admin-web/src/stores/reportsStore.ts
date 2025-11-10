import { create } from 'zustand';
import axios from 'axios';

const API_URL = 'http://localhost:5194/api';

export interface OverviewData {
  revenue: number;
  orderCount: number;
  avgOrderValue: number;
  completedOrders: number;
  customerCount: number;
  fromDate: string;
  toDate: string;
}

export interface RevenueByDateData {
  date: string;
  revenue: number;
  orderCount: number;
}

export interface TopItemData {
  menuItemId: string;
  menuItemName: string;
  totalQuantity: number;
  totalRevenue: number;
  orderCount: number;
}

export interface StaffPerformanceData {
  staffId: string;
  staffName: string;
  orderCount: number;
  completedOrders: number;
  totalRevenue: number;
}

export interface CategorySalesData {
  categoryId: string;
  categoryName: string;
  totalQuantity: number;
  totalRevenue: number;
  itemCount: number;
}

export interface PaymentMethodData {
  method: string;
  count: number;
  totalAmount: number;
}

interface ReportsState {
  overview: OverviewData | null;
  revenueByDate: RevenueByDateData[];
  topItems: TopItemData[];
  staffPerformance: StaffPerformanceData[];
  categorySales: CategorySalesData[];
  paymentMethods: PaymentMethodData[];
  isLoading: boolean;
  error: string | null;

  // Actions
  fetchOverview: (fromDate?: Date, toDate?: Date) => Promise<void>;
  fetchRevenueByDate: (fromDate?: Date, toDate?: Date) => Promise<void>;
  fetchTopItems: (fromDate?: Date, toDate?: Date, limit?: number) => Promise<void>;
  fetchStaffPerformance: (fromDate?: Date, toDate?: Date) => Promise<void>;
  fetchCategorySales: (fromDate?: Date, toDate?: Date) => Promise<void>;
  fetchPaymentMethods: (fromDate?: Date, toDate?: Date) => Promise<void>;
}

export const useReportsStore = create<ReportsState>((set) => ({
  overview: null,
  revenueByDate: [],
  topItems: [],
  staffPerformance: [],
  categorySales: [],
  paymentMethods: [],
  isLoading: false,
  error: null,

  fetchOverview: async (fromDate?: Date, toDate?: Date) => {
    set({ isLoading: true, error: null });
    try {
      const params: any = {};
      if (fromDate) params.fromDate = fromDate.toISOString();
      if (toDate) params.toDate = toDate.toISOString();
      
      const response = await axios.get<OverviewData>(`${API_URL}/Reports/overview`, { params });
      set({ overview: response.data, isLoading: false });
    } catch (error: any) {
      set({ error: error.message, isLoading: false });
      console.error('Error fetching overview:', error);
    }
  },

  fetchRevenueByDate: async (fromDate?: Date, toDate?: Date) => {
    try {
      const params: any = {};
      if (fromDate) params.fromDate = fromDate.toISOString();
      if (toDate) params.toDate = toDate.toISOString();
      
      const response = await axios.get<RevenueByDateData[]>(`${API_URL}/Reports/revenue-by-date`, { params });
      set({ revenueByDate: response.data });
    } catch (error: any) {
      console.error('Error fetching revenue by date:', error);
    }
  },

  fetchTopItems: async (fromDate?: Date, toDate?: Date, limit = 10) => {
    try {
      const params: any = { limit };
      if (fromDate) params.fromDate = fromDate.toISOString();
      if (toDate) params.toDate = toDate.toISOString();
      
      const response = await axios.get<TopItemData[]>(`${API_URL}/Reports/top-items`, { params });
      set({ topItems: response.data });
    } catch (error: any) {
      console.error('Error fetching top items:', error);
    }
  },

  fetchStaffPerformance: async (fromDate?: Date, toDate?: Date) => {
    try {
      const params: any = {};
      if (fromDate) params.fromDate = fromDate.toISOString();
      if (toDate) params.toDate = toDate.toISOString();
      
      const response = await axios.get<StaffPerformanceData[]>(`${API_URL}/Reports/staff-performance`, { params });
      set({ staffPerformance: response.data });
    } catch (error: any) {
      console.error('Error fetching staff performance:', error);
    }
  },

  fetchCategorySales: async (fromDate?: Date, toDate?: Date) => {
    try {
      const params: any = {};
      if (fromDate) params.fromDate = fromDate.toISOString();
      if (toDate) params.toDate = toDate.toISOString();
      
      const response = await axios.get<CategorySalesData[]>(`${API_URL}/Reports/category-sales`, { params });
      set({ categorySales: response.data });
    } catch (error: any) {
      console.error('Error fetching category sales:', error);
    }
  },

  fetchPaymentMethods: async (fromDate?: Date, toDate?: Date) => {
    try {
      const params: any = {};
      if (fromDate) params.fromDate = fromDate.toISOString();
      if (toDate) params.toDate = toDate.toISOString();
      
      const response = await axios.get<PaymentMethodData[]>(`${API_URL}/Reports/payment-methods`, { params });
      set({ paymentMethods: response.data });
    } catch (error: any) {
      console.error('Error fetching payment methods:', error);
    }
  },
}));
