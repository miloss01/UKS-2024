import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ChangePasswordDto, LoginCredentials, RegisterUserDto, StandardUser} from "../models/models";
import {Observable, tap} from "rxjs";
import {environment} from "../env/environment";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  changePassword(changePasswordDto: ChangePasswordDto): Observable<any>{
    return this.http.patch<any>(`${environment.apiHost}user/password/change`, changePasswordDto);
  }
  registerUser(registerUserDto: RegisterUserDto): Observable<StandardUser>{
    return this.http.post<StandardUser>(`${environment.apiHost}user`, registerUserDto);
  }
}
