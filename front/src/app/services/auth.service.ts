import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {LoginCredentials, UserData} from "../models/models";
import {Observable} from "rxjs";
import {environment} from "../env/environment";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public userInfo?: UserData;

  constructor(private http: HttpClient) { }

  login(credentials: LoginCredentials): Observable<UserData>{
    return this.http.post<UserData>(`${environment.apiHost}auth/login`, credentials, {
      withCredentials: true
    });
  }

}
