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

export interface MenuCategoryDto {
  id: string;
  name: string;
  displayOrder: number;
}

export interface MenuItemDto {
  id: string;
  name: string;
  description?: string;
  basePrice: number;
  categoryId: string;
  categoryName: string;
  status: string;
  optionGroups: MenuOptionGroupDto[];
}

export interface MenuOptionGroupDto {
  id: string;
  name: string;
  isRequired: boolean;
  options: MenuOptionDto[];
}

export interface MenuOptionDto {
  id: string;
  name: string;
  extraPrice: number;
  isDefault: boolean;
}

export interface TableDto {
  id: string;
  code: string;
  zone?: string;
  capacity: number;
  status: string;
}

export interface CreateOrderCommand {
  tableId: string;
  staffId: string;
  guestCount: number;
  adultCount: number;
  childCount: number;
  childHeights?: string | null;
  buffetType: string;
  hasDessertBuffet: boolean;
  notes?: string;
  items: CreateOrderItemDto[];
}

export interface CreateOrderItemDto {
  menuItemId: string;
  quantity: number;
  note?: string;
  options: CreateOrderItemOptionDto[];
}

export interface CreateOrderItemOptionDto {
  menuOptionId: string;
  quantity: number;
}

export interface UpdateOrderStatusRequest {
  status: string;
  actorId: string;
}

