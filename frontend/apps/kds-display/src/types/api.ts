export interface OrderDto {
  id: string;
  tableId: string;
  tableCode: string;
  staffId: string;
  staffName: string;
  status: string;
  guestCount: number;
  notes?: string;
  createdAt: string;
  closedAt?: string;
  items: OrderItemDto[];
}

export interface OrderItemDto {
  id: string;
  menuItemId: string;
  menuItemName: string;
  unitPrice: number;
  quantity: number;
  note?: string;
  status: string;
  options: OrderItemOptionDto[];
}

export interface OrderItemOptionDto {
  id: string;
  menuOptionId: string;
  optionName: string;
  extraPrice: number;
  quantity: number;
}

export interface KitchenTicket {
  id: string;
  orderId: string;
  tableCode: string;
  status: 'Pending' | 'InProgress' | 'Ready' | 'Delivered';
  createdAt: string;
  startedAt?: string;
  completedAt?: string;
  elapsed: string;
  items: KitchenTicketItem[];
}

export interface KitchenTicketItem {
  id: string;
  menuItemName: string;
  quantity: number;
  note?: string;
  status: 'Pending' | 'InProgress' | 'Ready' | 'Delivered';
  options: string[];
}
