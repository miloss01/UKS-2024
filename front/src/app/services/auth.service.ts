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

  constructor(private http: HttpClient) { }

  private loadUserData(token: string){
    const helper = new JwtHelperService();
    const decodedToken = helper.decodeToken(token);
    const id:string = decodedToken.nameid;
    const email:string = decodedToken.email;
    const role:UserRole = decodedToken.role as UserRole;
    this.userData = {
      userId: id,
      userEmail: email,
      userRole: role
    };
    console.log(this.userData);
  }

  login(credentials: LoginCredentials): Observable<{accessToken: string}>{
    return this.http.post<{accessToken: string}>(`${environment.apiHost}auth/login`, credentials, {
      withCredentials: true
    }).pipe(
      tap({
        next: resp => {
          localStorage.setItem('token', resp.accessToken);
          this.loadUserData(resp.accessToken);
        }
      })
    );
  }

}
