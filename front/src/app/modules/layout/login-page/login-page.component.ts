import { Component } from '@angular/core';
import {MatInputModule} from "@angular/material/input";
import {MatButtonModule} from "@angular/material/button";
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {NgIf} from "@angular/common";
import {LoginCredentials} from "../../../models/models";
import {AuthService} from "../../../services/auth.service";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  selector: 'app-login-page',
  standalone: true,
  imports: [
    MatInputModule,
    MatButtonModule,
    ReactiveFormsModule,
    NgIf
  ],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.css'
})
export class LoginPageComponent {
  errorMessage: string = "";
  loginForm:FormGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required])
  });

  constructor(private authService: AuthService) {
  }

  submitForm(){
    this.errorMessage = '';
    if(this.loginForm.valid){
      const credentials:LoginCredentials = {
        email: this.loginForm.controls['email'].value,
        password: this.loginForm.controls['password'].value,
        withCredentials: true
      }
      this.login(credentials);
    }
  }

  private login(credentials:LoginCredentials){
    this.authService.login(credentials).subscribe({
      next: (response) => {
        console.log(response);
      },
      error: (error) => {
        if(error instanceof HttpErrorResponse){
          this.errorMessage = error.error.message;
        }else{
          this.errorMessage = "Something went wrong"
        }
      }
    });
  }
}
