/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { UserRole } from './UserRole';
export type LoginResponceDto = {
    accessToken: string | null;
    expiresAt: string;
    login: string | null;
    roles: Array<UserRole> | null;
};

