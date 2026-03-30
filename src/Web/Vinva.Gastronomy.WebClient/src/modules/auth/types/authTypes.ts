import type { UserRole} from "@/api";

export interface User {
    id: string;
    name: string | null;
    email: string | null;    
    roles: Array<UserRole> | null;
}

export interface AuthOperationResult
{
    isSuccess: boolean;
    erros: string | null;
    successInfo: string | null;
}