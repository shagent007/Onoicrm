import vue from '@vitejs/plugin-vue';
import { join } from 'path';
import { defineConfig } from 'vite';
export default defineConfig({
  plugins: [vue()],
  resolve: {
    alias: {
      '@': join(__dirname, 'src'),
    },
  },
  server: {
    host: true,
    port: 3000,
    proxy: {
      '/api/v1': {
        target: "http://localhost:5180",
        changeOrigin: true,
        rewrite: (path) => {
          return path.replace('/^/api/v1', '');
        },
      },
    },
  },
});
