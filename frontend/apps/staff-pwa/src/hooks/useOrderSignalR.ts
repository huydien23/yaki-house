import { useEffect, useRef } from 'react';
import * as signalR from '@microsoft/signalr';
import { useOrderStore } from '../stores/orderStore';
import { OrderDto } from '../types/api';

const HUB_URL = 'http://localhost:5194/hubs/orders';

export const useOrderSignalR = (enabled: boolean = true) => {
  const connectionRef = useRef<signalR.HubConnection | null>(null);
  const { updateOrderInList } = useOrderStore();

  useEffect(() => {
    if (!enabled) return;

    // Create connection
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(HUB_URL)
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    // Event handlers
    connection.on('OrderCreated', (order: OrderDto) => {
      console.log('New order created:', order);
      updateOrderInList(order);
    });

    connection.on('OrderUpdated', (order: OrderDto) => {
      console.log('Order updated:', order);
      updateOrderInList(order);
    });

    connection.on('OrderStatusChanged', (data: { orderId: string; status: string }) => {
      console.log('Order status changed:', data);
      // Fetch updated order details
      useOrderStore.getState().fetchOrderById(data.orderId);
    });

    // Start connection
    const startConnection = async () => {
      try {
        await connection.start();
        console.log('SignalR Connected to Order Hub');
      } catch (err) {
        console.error('SignalR Connection Error:', err);
        setTimeout(startConnection, 5000); // Retry after 5 seconds
      }
    };

    startConnection();
    connectionRef.current = connection;

    // Cleanup
    return () => {
      if (connectionRef.current) {
        connectionRef.current.stop();
      }
    };
  }, [enabled, updateOrderInList]);

  return connectionRef.current;
};
