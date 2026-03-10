/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { UserRole } from './UserRole';
export type LoginResponce = {
    accessToken: string | null;
    refreshToken: string | null;
    expiresAt: string;
    login: string | null;
    role: UserRole;
};

