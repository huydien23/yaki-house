import { create } from 'zustand';
import type { OrderDto, CreateOrderCommand } from '../types/api';
import { ordersApi } from '../services/api';

interface OrderState {
  orders: OrderDto[];
  currentOrder: OrderDto | null;
  isLoading: boolean;
  error: string | null;
  
  // Actions
  fetchOrders: () => Promise<void>;
  fetchOrderById: (id: string) => Promise<void>;
  createOrder: (command: CreateOrderCommand) => Promise<OrderDto | null>;
  updateOrderStatus: (id: string, status: string, actorId: string) => Promise<void>;
  setCurrentOrder: (order: OrderDto | null) => void;
  updateOrderInList: (order: OrderDto) => void;
}

export const useOrderStore = create<OrderState>((set, get) => ({
  orders: [],
  currentOrder: null,
  isLoading: false,
  error: null,

  fetchOrders: async () => {
    set({ isLoading: true, error: null });
    try {
      const orders = await ordersApi.getActiveOrders();
      set({ orders, isLoading: false });
    } catch (error) {
      set({ error: 'Failed to fetch orders', isLoading: false });
      console.error('Error fetching orders:', error);
    }
  },

  fetchOrderById: async (id: string) => {
    set({ isLoading: true, error: null });
    try {
      const order = await ordersApi.getOrderById(id);
      set({ currentOrder: order, isLoading: false });
    } catch (error) {
      set({ error: 'Failed to fetch order', isLoading: false });
      console.error('Error fetching order:', error);
    }
  },

  createOrder: async (command: CreateOrderCommand) => {
    set({ isLoading: true, error: null });
    try {
      const order = await ordersApi.createOrder(command);
      set((state) => ({
        orders: [order, ...state.orders],
        currentOrder: order,
        isLoading: false,
      }));
      return order;
    } catch (error) {
      set({ error: 'Failed to create order', isLoading: false });
      console.error('Error creating order:', error);
      return null;
    }
  },

  updateOrderStatus: async (id: string, status: string, actorId: string) => {
    set({ isLoading: true, error: null });
    try {
      await ordersApi.updateOrderStatus(id, { status, actorId });
      set((state) => ({
        orders: state.orders.map((o) =>
          o.id === id ? { ...o, status } : o
        ),
        currentOrder:
          state.currentOrder?.id === id
            ? { ...state.currentOrder, status }
            : state.currentOrder,
        isLoading: false,
      }));
    } catch (error) {
      set({ error: 'Failed to update order status', isLoading: false });
      console.error('Error updating order status:', error);
    }
  },

  setCurrentOrder: (order: OrderDto | null) => {
    set({ currentOrder: order });
  },

  updateOrderInList: (order: OrderDto) => {
    set((state) => ({
      orders: state.orders.map((o) => (o.id === order.id ? order : o)),
      currentOrder:
        state.currentOrder?.id === order.id ? order : state.currentOrder,
    }));
  },
}));

