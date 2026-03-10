/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { GetCurrentUserQueryResponse } from '../models/GetCurrentUserQueryResponse';
import type { LoginRequest } from '../models/LoginRequest';
import type { LoginResponceResult } from '../models/LoginResponceResult';
import type { LogoutRequest } from '../models/LogoutRequest';
import type { RefreshTokenRequest } from '../models/RefreshTokenRequest';
import type { RefreshTokenResponce } from '../models/RefreshTokenResponce';
import type { ValidateTokenRequest } from '../models/ValidateTokenRequest';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class AuthenticationService {
    /**
     * @param requestBody
     * @returns LoginResponceResult OK
     * @throws ApiError
     */
    public static postLogin(
        requestBody?: LoginRequest,
    ): CancelablePromise<LoginResponceResult> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/login',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param requestBody
     * @returns RefreshTokenResponce OK
     * @throws ApiError
     */
    public static postRefresh(
        requestBody?: RefreshTokenRequest,
    ): CancelablePromise<RefreshTokenResponce> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/refresh',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param requestBody
     * @returns any OK
     * @throws ApiError
     */
    public static postValidate(
        requestBody?: ValidateTokenRequest,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/validate',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @returns GetCurrentUserQueryResponse OK
     * @throws ApiError
     */
    public static getMe(): CancelablePromise<GetCurrentUserQueryResponse> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/me',
        });
    }
    /**
     * @param requestBody
     * @returns any OK
     * @throws ApiError
     */
    public static postLogout(
        requestBody?: LogoutRequest,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/logout',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
}
