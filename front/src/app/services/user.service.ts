import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ChangePasswordDto, LoginCredentials} from "../models/models";
import {Observable, tap} from "rxjs";
import {environment} from "../env/environment";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  changePassword(changePasswordDto: ChangePasswordDto): Observable<any>{
    return this.http.patch<any>(`${environment.apiHost}user/password/change`, changePasswordDto)
  }
}
