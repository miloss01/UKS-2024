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

export interface TeamsData {
  Name: string,
  Description: string
}

export enum UserRole{
  StandardUser = "StandardUser",
  Admin = "Admin",
  SuperAdmin = "SuperAdmin"
}
