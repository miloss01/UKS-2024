export default interface RepositoryCreation {
    name:string
    namespace:string
    description:string
    visibility:string
}

export interface Repository extends RepositoryCreation {
  id: number,
  images: Image[]
  lastPushed: string
  createdAt: string
}

export interface Image {
    name: string
    tags: string[]
    pushed: string
}

export interface LoginCredentials {
  email: string,
  password: string,
  withCredentials: boolean
}
export interface UserData{
  userId: string,
  userEmail: string,
  userRole: UserRole
}

export enum UserRole{
  StandardUser = "StandardUser",
  Admin = "Admin",
  SuperAdmin = "SuperAdmin"
}
