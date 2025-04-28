import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    port: parseInt(process.env.PORT) || 3000,
    host: true
  },
  define: {
    'import.meta.env.VITE_CHAT_API': JSON.stringify(process.env['services__moviessemanticsearch-api__https__0'])
  },
})
