import * as signalR from '@microsoft/signalr';

const HUB_BASE_URL = import.meta.env.VITE_API_URL?.replace('/api', '') || 'https://localhost:7001';

class SignalRService {
  private orderConnection: signalR.HubConnection | null = null;
  private kitchenConnection: signalR.HubConnection | null = null;

  async startOrderConnection(): Promise<void> {
    if (this.orderConnection) {
      return;
    }

    this.orderConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${HUB_BASE_URL}/hubs/orders`)
      .withAutomaticReconnect()
      .build();

    try {
      await this.orderConnection.start();
      console.log('SignalR Order Hub connected');
    } catch (err) {
      console.error('Error connecting to Order Hub:', err);
    }
  }

  async startKitchenConnection(): Promise<void> {
    if (this.kitchenConnection) {
      return;
    }

    this.kitchenConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${HUB_BASE_URL}/hubs/kitchen`)
      .withAutomaticReconnect()
      .build();

    try {
      await this.kitchenConnection.start();
      console.log('SignalR Kitchen Hub connected');
    } catch (err) {
      console.error('Error connecting to Kitchen Hub:', err);
    }
  }

  async stopOrderConnection(): Promise<void> {
    if (this.orderConnection) {
      await this.orderConnection.stop();
      this.orderConnection = null;
    }
  }

  async stopKitchenConnection(): Promise<void> {
    if (this.kitchenConnection) {
      await this.kitchenConnection.stop();
      this.kitchenConnection = null;
    }
  }

  onOrderCreated(callback: (data: any) => void): void {
    this.orderConnection?.on('OrderCreated', callback);
  }

  onOrderUpdated(callback: (data: any) => void): void {
    this.orderConnection?.on('OrderUpdated', callback);
  }

  onOrderStatusChanged(callback: (data: { orderId: string; status: string }) => void): void {
    this.orderConnection?.on('OrderStatusChanged', callback);
  }

  onTicketStatusChanged(callback: (data: { ticketId: string; status: string }) => void): void {
    this.kitchenConnection?.on('TicketStatusChanged', callback);
  }

  onItemStatusChanged(callback: (data: { itemId: string; status: string }) => void): void {
    this.kitchenConnection?.on('ItemStatusChanged', callback);
  }

  async joinTableGroup(tableId: string): Promise<void> {
    await this.orderConnection?.invoke('JoinTableGroup', tableId);
  }

  async leaveTableGroup(tableId: string): Promise<void> {
    await this.orderConnection?.invoke('LeaveTableGroup', tableId);
  }

  async joinKitchenGroup(): Promise<void> {
    await this.kitchenConnection?.invoke('JoinKitchenGroup');
  }

  async joinStationGroup(stationId: string): Promise<void> {
    await this.kitchenConnection?.invoke('JoinStationGroup', stationId);
  }
}

export const signalRService = new SignalRService();

