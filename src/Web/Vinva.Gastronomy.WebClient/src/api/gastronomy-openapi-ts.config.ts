import { defineConfig } from '@hey-api/openapi-ts';




export default defineConfig({
  plugins: ['@hey-api/client-fetch'], 
  input: './src/api/gastronomy-openapi.json', 
  output: './src/api/gastronomy_generated'
});