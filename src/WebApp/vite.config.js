import { defineConfig } from 'vite';
import path from 'path';

export default defineConfig({
  root: './Assets',
  base: '/',
  build: {
    outDir: '../wwwroot/dist',
    emptyOutDir: true,
    manifest: true,
    rollupOptions: {
      input: {
        main: path.resolve(__dirname, 'Assets/main.js')
      },
      output: {
        entryFileNames: 'js/[name].min.js',
        chunkFileNames: 'js/[name]-[hash].js',
        assetFileNames: (assetInfo) => {
          if (assetInfo.name.endsWith('.css')) {
            return 'css/[name].min[extname]';
          }
          return 'assets/[name]-[hash][extname]';
        }
      }
    }
  },
  server: {
    port: 7005,
    strictPort: true,
    hmr: {
      protocol: 'ws',
      host: 'localhost'
    }
  }
});
