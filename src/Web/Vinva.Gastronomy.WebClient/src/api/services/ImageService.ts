/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CreateImageCommandResponse } from '../models/CreateImageCommandResponse';
import type { GetImagesByIdsQuery } from '../models/GetImagesByIdsQuery';
import type { GetImagesByIdsQueryResponse } from '../models/GetImagesByIdsQueryResponse';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class ImageService {
    /**
     * @param requestBody
     * @returns GetImagesByIdsQueryResponse OK
     * @throws ApiError
     */
    public static postApiImagesGetByIds(
        requestBody?: GetImagesByIdsQuery,
    ): CancelablePromise<GetImagesByIdsQueryResponse> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Images/GetByIds',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param formData
     * @returns CreateImageCommandResponse OK
     * @throws ApiError
     */
    public static postApiImagesCreate(
        formData?: {
            File: Blob;
            Group?: string;
        },
    ): CancelablePromise<CreateImageCommandResponse> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Images/Create',
            formData: formData,
            mediaType: 'multipart/form-data',
        });
    }
}
