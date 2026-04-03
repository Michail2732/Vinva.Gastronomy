export class ApiGastronomyError extends Error {
    constructor(message: string) {
        super(message);
        this.name = 'ApiGastronomyError';
        Object.setPrototypeOf(this, ApiGastronomyError.prototype);
    }
}


export type SuccessResult<T> = {
  isSuccess: true;
  data: T;
}

// Универсальная обёртка для ошибки
export type ErrorResult = {
  isSuccess: false;
  error: string;
}

// Результирующий тип
export type ApiDataResult<T> = SuccessResult<T> | ErrorResult;
export type ApiResult = 
{
  isSuccess: boolean;
  error?: string;
}