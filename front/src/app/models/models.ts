export default interface RepositoryCreation {
    name:string
    owner:string
    description:string
    isPublic: boolean
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

export interface DockerImageDTO {
  imageId: string;
  repositoryName: string;
  repositoryId: string;
  badge: string;
  starCount: number;
  description: string;
  tags: string[];
  lastPush: string;
  owner: string;
  createdAt: string;
}

export interface PageDTO<T> {
  data: T[];
  totalNumberOfElements: number;
}

export interface DockerRepositoryDTO {
  id: string;
  name: string;
  description: string;
  isPublic: boolean;
  starCount: number;
  badge: string;
  images: DockerImageDTO[];
  owner: string;
  createdAt: string;
}
