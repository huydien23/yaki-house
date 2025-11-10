import { create } from 'zustand';
import axios from 'axios';
import { OrderDto, KitchenTicket } from '../types/api';

const API_URL = 'http://localhost:5194/api';

interface KitchenState {
  tickets: KitchenTicket[];
  isLoading: boolean;
  error: string | null;
  
  // Actions
  fetchActiveOrders: () => Promise<void>;
  updateTicketStatus: (ticketId: string, status: string) => void;
  addTicket: (order: OrderDto) => void;
  updateTicket: (order: OrderDto) => void;
  removeTicket: (orderId: string) => void;
}

// Helper function to calculate elapsed time
const calculateElapsed = (createdAt: string): string => {
  const created = new Date(createdAt);
  const now = new Date();
  const diff = Math.floor((now.getTime() - created.getTime()) / 1000); // seconds
  
  const minutes = Math.floor(diff / 60);
  const seconds = diff % 60;
  
  return `${String(minutes).padStart(2, '0')}:${String(seconds).padStart(2, '0')}`;
};

// Convert OrderDto to KitchenTicket
const orderToTicket = (order: OrderDto): KitchenTicket => {
  return {
    id: order.id,
    orderId: order.id,
    tableCode: order.tableCode,
    status: order.status === 'Draft' ? 'Pending' : 
            order.status === 'Confirmed' ? 'InProgress' :
            order.status === 'Ready' ? 'Ready' : 'Delivered',
    createdAt: order.createdAt,
    elapsed: calculateElapsed(order.createdAt),
    items: order.items.map(item => ({
      id: item.id,
      menuItemName: item.menuItemName,
      quantity: item.quantity,
      note: item.note,
      status: item.status === 'Pending' ? 'Pending' :
              item.status === 'InProgress' ? 'InProgress' :
              item.status === 'Ready' ? 'Ready' : 'Delivered',
      options: item.options.map(opt => `${opt.optionName} (${opt.quantity}x)`)
    }))
  };
};

export const useKitchenStore = create<KitchenState>((set, get) => ({
  tickets: [],
  isLoading: false,
  error: null,

  fetchActiveOrders: async () => {
    set({ isLoading: true, error: null });
    try {
      const response = await axios.get<{ data: OrderDto[] }>(`${API_URL}/Orders`);
      const tickets = response.data.data
        .filter(order => order.status !== 'Completed' && order.status !== 'Cancelled')
        .map(orderToTicket);
      set({ tickets, isLoading: false });
    } catch (error: any) {
      console.error('Failed to fetch orders:', error);
      set({ error: error.message, isLoading: false });
    }
  },

  updateTicketStatus: (ticketId: string, status: string) => {
    set(state => ({
      tickets: state.tickets.map(ticket =>
        ticket.id === ticketId ? { ...ticket, status: status as any } : ticket
      )
    }));
  },

  addTicket: (order: OrderDto) => {
    const ticket = orderToTicket(order);
    set(state => ({
      tickets: [ticket, ...state.tickets]
    }));
  },

  updateTicket: (order: OrderDto) => {
    const ticket = orderToTicket(order);
    set(state => ({
      tickets: state.tickets.map(t =>
        t.id === order.id ? ticket : t
      )
    }));
  },

  removeTicket: (orderId: string) => {
    set(state => ({
      tickets: state.tickets.filter(t => t.id !== orderId)
    }));
  }
}));

// Update elapsed time every second
setInterval(() => {
  const store = useKitchenStore.getState();
  if (store.tickets.length > 0) {
    useKitchenStore.setState({
      tickets: store.tickets.map(ticket => ({
        ...ticket,
        elapsed: calculateElapsed(ticket.createdAt)
      }))
    });
  }
}, 1000);
