import { defineConfig } from '@hey-api/openapi-ts';
import { createClient } from '@hey-api/client-fetch'




export default defineConfig({
  client: '@hey-api/client-fetch', 
  input: './openapi.json', 
  output: './src/api', 
  client: 'fetch'
});