export default interface RepositoryCreation {
  id: string,
  name:string
  namespace:string
  description:string
  visibility:string
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

export interface TeamsData {
  id: string,
  name: string,
  description: string
  members: Member[],
  organizationId: string
}

export interface Member {
  email: string
}

export interface TeamRepoPerm {
  id: string,
  permission: number,
  teamId: string,
  team: TeamsData,
  repositoryId: string,
  repository: RepositoryCreation
}

export enum UserRole{
  StandardUser = "StandardUser",
  Admin = "Admin",
  SuperAdmin = "SuperAdmin"
}
