import { useEffect, useRef } from 'react';
import * as signalR from '@microsoft/signalr';
import { useKitchenStore } from '../stores/kitchenStore';
import { OrderDto } from '../types/api';

const HUB_URL = 'http://localhost:5194/hubs/kitchen';

export const useSignalR = () => {
  const connectionRef = useRef<signalR.HubConnection | null>(null);
  const { addTicket, updateTicket, removeTicket, updateTicketStatus } = useKitchenStore();

  useEffect(() => {
    // Create connection
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(HUB_URL)
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    // Event handlers
    connection.on('NewOrder', (order: OrderDto) => {
      console.log('New order received:', order);
      addTicket(order);
    });

    connection.on('OrderUpdated', (order: OrderDto) => {
      console.log('Order updated:', order);
      updateTicket(order);
    });

    connection.on('OrderStatusChanged', (data: { orderId: string; status: string }) => {
      console.log('Order status changed:', data);
      if (data.status === 'Completed' || data.status === 'Cancelled') {
        removeTicket(data.orderId);
      } else {
        updateTicketStatus(data.orderId, data.status);
      }
    });

    connection.on('TicketStatusChanged', (data: { ticketId: string; status: string }) => {
      console.log('Ticket status changed:', data);
      updateTicketStatus(data.ticketId, data.status);
    });

    connection.on('ItemStatusChanged', (data: { itemId: string; status: string }) => {
      console.log('Item status changed:', data);
      // Update item status in the ticket
    });

    // Start connection
    const startConnection = async () => {
      try {
        await connection.start();
        console.log('SignalR Connected to Kitchen Hub');
        
        // Join kitchen group
        await connection.invoke('JoinKitchenGroup');
        console.log('Joined kitchen group');
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
  }, [addTicket, updateTicket, removeTicket, updateTicketStatus]);

  return connectionRef.current;
};
