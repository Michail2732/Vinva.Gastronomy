import type { UserRole } from "@/api/types.gen"

export interface User {
  id: string
  name: string | null
  email: string  | null
  roles: Array<UserRole> | null
}