export interface UserLoginReponse
{
    token: string,
    user: User
}


export class AuthError extends Error
{
    constructor(message: string)
    {
      super(message);
    }
}

export interface User {
  id: number
  name: string
  email: string  
  roles: string[],
  createdAt: string
}

export type RecipeCard = {
    title: string,
    imageSrc: string,   
}