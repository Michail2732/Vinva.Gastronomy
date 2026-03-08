import axios from 'axios'

const apiClient = axios.create({
    baseURL: import.meta.env.VITE_API_URL,
    timeout: 10000,
    Headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    }
});

apiClient.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('authToken');
        if (token)
        {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
)

apiClient.interceptors.response.use(
    (response) => response.data,
    async (error) => {
        const originalRequest = error.config;

        if (error.response?.status === 401 && !originalRequest._retry) 
        {
            originalRequest._retry = true;
      
            try
            {
                // Пытаемся обновить токен
                const refreshToken = localStorage.getItem('refreshToken');
                const response = await axios.post('/auth/refresh', {
                refreshToken
                });
                
                // Сохраняем новый токен
                localStorage.setItem('authToken', response.data.token);
                
                // Обновляем заголовок и повторяем исходный запрос
                originalRequest.headers.Authorization = `Bearer ${response.data.token}`;
                return apiClient(originalRequest);
            }
            catch (refreshError) 
            {
                // Не удалось обновить токен — на страницу логина
                window.location.href = '/login';
                return Promise.reject(refreshError);
            }
        }
    
        return Promise.reject(error);
    }
);