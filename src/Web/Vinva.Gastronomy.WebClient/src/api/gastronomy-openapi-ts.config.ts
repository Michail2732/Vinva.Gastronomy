import { defineConfig } from '@hey-api/openapi-ts';




export default defineConfig({    
  input: './src/api/gastronomy-openapi.json', 
  output: './src/api/gastronomy_generated',
  plugins: 
  [
    {
      name: '@hey-api/typescript',
      enums: 'typescript' 
    },
    '@hey-api/client-fetch',    
    '@hey-api/sdk'
  ]
});