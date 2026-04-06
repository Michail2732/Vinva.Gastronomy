import {client } from './gastronomy_generated/client.gen';
import { ApiGastronomyError } from './types';

async function getErrorMessage(response: Response): Promise<string> {
    try {
        const body = await response.clone().json();
        return body?.message || body?.error || body?.detail || response.statusText;
    } catch {
        return response.statusText;
    }
}

client.setConfig({
  baseUrl: import.meta.env.VITE_API_URL,
  credentials: 'include',
  throwOnError: true,
  fetch: async (input, init) =>
    {
        try 
        {
            const response = await fetch(input, 
                {
                    ...init,
                    credentials: 'include'
                });
            if (!response.ok) 
            {
                const message = await getErrorMessage(response);
                throw new ApiGastronomyError(message);
            }        
            return response;
        }
        catch (error) 
        {
             // Сетевая ошибка (нет интернета, таймаут, DNS ошибка)
            if (error instanceof TypeError) {
                throw new ApiGastronomyError('Network error: Unable to connect to server');
            }
            
            // Ошибка парсинга JSON (редко, но может быть)
            if (error instanceof SyntaxError) {
                throw new ApiGastronomyError('Invalid response format from server');
            }
            
            // Уже наша ошибка
            if (error instanceof ApiGastronomyError) {
                throw error;
            }
            
            // Любая другая ошибка
            if (error instanceof Error) {
                throw new ApiGastronomyError(error.message);
            }
            
            // Совсем неизвестная ошибка (fallback)
            throw new ApiGastronomyError('An unknown error occurred');
        }        
    },
});