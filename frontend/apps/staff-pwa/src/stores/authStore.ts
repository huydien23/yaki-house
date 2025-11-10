import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import axios from 'axios';

interface User {
  id: string;
  username: string;
  fullName: string;
  email: string;
  role: string;
}

interface AuthState {
  user: User | null;
  accessToken: string | null;
  refreshToken: string | null;
  isAuthenticated: boolean;
  login: (username: string, password: string) => Promise<void>;
  logout: () => void;
  refreshAccessToken: () => Promise<void>;
  setUser: (user: User, accessToken: string, refreshToken: string) => void;
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set, get) => ({
      user: null,
      accessToken: null,
      refreshToken: null,
      isAuthenticated: false,

      login: async (username: string, password: string) => {
        try {
          const response = await axios.post('http://localhost:5194/api/Auth/login', {
            username,
            password,
          });

          const { user, accessToken, refreshToken } = response.data;
          
          set({
            user,
            accessToken,
            refreshToken,
            isAuthenticated: true,
          });

          // Set default Authorization header
          axios.defaults.headers.common['Authorization'] = `Bearer ${accessToken}`;
        } catch (error) {
          console.error('Login failed:', error);
          throw error;
        }
      },

      logout: () => {
        set({
          user: null,
          accessToken: null,
          refreshToken: null,
          isAuthenticated: false,
        });
        delete axios.defaults.headers.common['Authorization'];
      },

      refreshAccessToken: async () => {
        const { accessToken, refreshToken } = get();
        
        if (!refreshToken) {
          throw new Error('No refresh token available');
        }

        try {
          const response = await axios.post('http://localhost:5194/api/Auth/refresh', {
            accessToken,
            refreshToken,
          });

          const { accessToken: newAccessToken, refreshToken: newRefreshToken } = response.data;
          
          set({
            accessToken: newAccessToken,
            refreshToken: newRefreshToken,
          });

          axios.defaults.headers.common['Authorization'] = `Bearer ${newAccessToken}`;
        } catch (error) {
          console.error('Token refresh failed:', error);
          get().logout();
          throw error;
        }
      },

      setUser: (user: User, accessToken: string, refreshToken: string) => {
        set({
          user,
          accessToken,
          refreshToken,
          isAuthenticated: true,
        });
        axios.defaults.headers.common['Authorization'] = `Bearer ${accessToken}`;
      },
    }),
    {
      name: 'auth-storage',
    }
  )
);
