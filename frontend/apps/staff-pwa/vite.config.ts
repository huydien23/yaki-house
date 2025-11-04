import { defineConfig } from "vite";
import react from "@vitejs/plugin-react-swc";
import { VitePWA } from "vite-plugin-pwa";

export default defineConfig({
  base: "./",
  plugins: [
    react(),
    VitePWA({
      registerType: "prompt",
      manifest: {
        name: "Yakihouse Staff",
        short_name: "Yakihouse",
        start_url: "/",
        display: "standalone",
        background_color: "#111827",
        theme_color: "#f97316"
      }
    })
  ],
  server: {
    port: 5174,
    host: true
  }
});

