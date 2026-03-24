import type { UserRole, UserState } from "@/api";

export interface User {
    id: string;
    login: string | null;
    email: string | null;
    state: UserState;
    roles: Array<UserRole> | null;
}