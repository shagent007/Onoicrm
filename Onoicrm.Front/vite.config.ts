import { sentryVitePlugin } from "@sentry/vite-plugin";
import { UserConfig } from "vite";
import Vue from "@vitejs/plugin-vue";
import path from "path";
import svgLoader from 'vite-svg-loader'
const config: UserConfig = {
  plugins: [Vue(), svgLoader()],

  resolve: {
    alias: {
      "~": path.resolve(__dirname, "src"),
    },
  },

  server: {
    host: true,
    port: 3030,
    proxy: {
      "/api/v1": {
        target: "https://onoicrm.com",
        changeOrigin: true,
        rewrite: (path) => path.replace("/^/api/v1", ""),
      },
    },
  }
};

export default config;