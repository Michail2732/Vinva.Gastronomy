import { CreateClientConfig } from '@/api/client/client.gen'

export const createClientConfig: CreateClientConfig = (config) => ({
    ...config,
    baseUrl: import.meta.env.VITE_API_URL,
    timeout: import.meta.env.VITE_API_TIMEOUT
})