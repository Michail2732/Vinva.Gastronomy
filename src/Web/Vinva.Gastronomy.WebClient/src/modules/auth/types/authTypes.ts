import type { UserRole, UserState} from "@/api/gastronomy_generated";

export interface User {
    id: string;
    login: string | null;
    email: string | null;
    state: UserState;
    roles: Array<UserRole> | null;
}

export interface AuthOperationResult
{
    isSuccess: boolean;
    error: string | null;
    details?: string | null | undefined;
}