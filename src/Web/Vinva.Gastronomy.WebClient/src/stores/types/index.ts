import { UserRole } from "@/api/types.gen"

export interface User {
  id: string
  name: string
  email: string  
  roles: Array<UserRole>
}