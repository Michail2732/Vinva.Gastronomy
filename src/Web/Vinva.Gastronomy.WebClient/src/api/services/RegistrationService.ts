/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { RegisterCommand } from '../models/RegisterCommand';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class RegistrationService {
    /**
     * @param requestBody
     * @returns any OK
     * @throws ApiError
     */
    public static postApiRegistrationRegister(
        requestBody?: RegisterCommand,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Registration/Register',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param tokenId
     * @returns any OK
     * @throws ApiError
     */
    public static postApiRegistrationRegisterConfirm(
        tokenId?: string,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Registration/RegisterConfirm',
            query: {
                'tokenId': tokenId,
            },
        });
    }
}
