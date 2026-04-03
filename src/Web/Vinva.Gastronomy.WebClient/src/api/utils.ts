import { ApiGastronomyError, type ApiDataResult } from "./types";

export async function safeApiCall<TData>(request: Promise<TData>) : Promise<ApiDataResult<TData>> {
  try {
    const data = await request;
    return {
      isSuccess: true,
      data: data
    };
  } catch (error) {
    if (error instanceof ApiGastronomyError) {
      return {
        isSuccess: false,
        error: error.message
      };
    }    
    throw error;
  }
}

export function getErrorMessage(err: any, defaultMessage: string = 'Ошибка') : string 
{
    return  err.response?.data?.message || err.Message || err.message || String(err) || defaultMessage;
}