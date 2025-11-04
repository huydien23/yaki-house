/** @type {import('tailwindcss').Config} */
const cerulean = {
  50: "#EDF9FF",
  100: "#D6EFFF",
  200: "#B5E5FF",
  300: "#82D7FF",
  400: "#48BEFF",
  500: "#1E9CFF",
  600: "#067CFF",
  700: "#0064F5",
  800: "#074CBB",
  900: "#0D479B",
  950: "#0D2C5E"
};

module.exports = {
  content: [
    "./apps/**/index.html",
    "./apps/**/*.{ts,tsx,js,jsx}"
  ],
  theme: {
    extend: {
      colors: {
        cerulean,
        primary: cerulean
      },
      fontFamily: {
        sans: ["Inter", "system-ui", "-apple-system", "BlinkMacSystemFont", "Segoe UI", "sans-serif"],
        display: ["Poppins", "Inter", "sans-serif"]
      },
      boxShadow: {
        floating: "0 20px 45px -25px rgba(7, 76, 187, 0.45)",
        glow: "0 28px 60px -30px rgba(0, 20, 61, 0.55)"
      }
    }
  },
  plugins: []
};

