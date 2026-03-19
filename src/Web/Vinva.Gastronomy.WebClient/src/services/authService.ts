import { type UserLoginReponse, type User, AuthError } from "@/types"

const mockUsers = [
  {
    id: 1,
    name: 'Иван Петров',
    email: 'ivan@example.com',
    password: "1234",
    roles: ['Client', 'Manager'] ,
    createdAt: '2024-01-15'
  },
  {
    id: 2,
    name: 'Мария Иванова',
    email: 'maria@example.com',    
    password: "5678",
    roles: ['Admin'],
    createdAt: '2024-01-10'
  }
]

const delay = (ms: number) => new Promise(resolve => setTimeout(resolve, ms))

const generateToken = (userId: number) => {
  return `fake-jwt-token-${userId}-${Date.now()}`
}


export const authService = {
    async login(email: string, pass: string) : Promise<UserLoginReponse>
    {
        await delay(500);
        const findedUser = mockUsers.find(a => a.email === email && a.password === pass);
        if (!findedUser)
             throw new AuthError("Логин или пароль неверные");

        return {
            token: generateToken(findedUser.id),
            user: findedUser
        }
    }


    
}