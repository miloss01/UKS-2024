import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {LoginCredentials, UserData, UserRole} from "../models/models";
import {Observable, tap} from "rxjs";
import {environment} from "../env/environment";
import {JwtHelperService} from "@auth0/angular-jwt";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public userData?: UserData;
  private static tokenStorageName: string = "token";

  constructor(private http: HttpClient) {
    this.loadUserData();
  }

  private loadUserData(){
    const token: string | null = localStorage.getItem(AuthService.tokenStorageName);
    if(!token){
      return;
    }
    const helper = new JwtHelperService();
    const decodedToken = helper.decodeToken(token);
    const id:string = decodedToken.nameid;
    const email:string = decodedToken.email;
    const isVerified:boolean = decodedToken.isVerified.toLowerCase() == "true";
    const role:UserRole = decodedToken.role as UserRole;
    this.userData = {
      userId: id,
      userEmail: email,
      userRole: role,
      isVerified: isVerified
    };
  }

  login(credentials: LoginCredentials): Observable<{accessToken: string}>{
    return this.http.post<{accessToken: string}>(`${environment.apiHost}auth/login`, credentials, {
      withCredentials: true
    }).pipe(
      tap({
        next: resp => {
          localStorage.setItem(AuthService.tokenStorageName, resp.accessToken);
          this.loadUserData();
        }
      })
    );
  }

  logout(){
    localStorage.removeItem(AuthService.tokenStorageName);
  }

  getAccessToken(): string | null{
    return localStorage.getItem(AuthService.tokenStorageName);
  }
}
