import { create } from 'zustand';
import type { TableDto } from '../types/api';
import { tablesApi } from '../services/api';

interface TableState {
  tables: TableDto[];
  zones: string[];
  selectedZone: string | null;
  selectedTable: TableDto | null;
  isLoading: boolean;
  error: string | null;
  
  // Actions
  fetchTables: (zone?: string) => Promise<void>;
  fetchZones: () => Promise<void>;
  setSelectedZone: (zone: string | null) => void;
  setSelectedTable: (table: TableDto | null) => void;
}

export const useTableStore = create<TableState>((set) => ({
  tables: [],
  zones: [],
  selectedZone: null,
  selectedTable: null,
  isLoading: false,
  error: null,

  fetchTables: async (zone?: string) => {
    set({ isLoading: true, error: null });
    try {
      const tables = await tablesApi.getTables(zone);
      set({ tables, isLoading: false });
    } catch (error) {
      set({ error: 'Failed to fetch tables', isLoading: false });
      console.error('Error fetching tables:', error);
    }
  },

  fetchZones: async () => {
    set({ isLoading: true, error: null });
    try {
      const zones = await tablesApi.getZones();
      set({ zones, isLoading: false });
    } catch (error) {
      set({ error: 'Failed to fetch zones', isLoading: false });
      console.error('Error fetching zones:', error);
    }
  },

  setSelectedZone: (zone: string | null) => {
    set({ selectedZone: zone });
  },

  setSelectedTable: (table: TableDto | null) => {
    set({ selectedTable: table });
  },
}));

