import { Component } from '@angular/core';
import {MatButtonModule} from "@angular/material/button";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {NgIf} from "@angular/common";
import {
  AbstractControl, AsyncValidatorFn,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators
} from "@angular/forms";
import {AuthService} from "../../../services/auth.service";
import {Router} from "@angular/router";
import {LoginCredentials, RegisterUserDto} from "../../../models/models";
import {HttpErrorResponse} from "@angular/common/http";
import {Observable, of} from "rxjs";
import {UserService} from "../../../services/user.service";

@Component({
  selector: 'app-register-page',
  standalone: true,
    imports: [
        MatButtonModule,
        MatFormFieldModule,
        MatInputModule,
        NgIf,
        ReactiveFormsModule
    ],
  templateUrl: './register-page.component.html',
  styleUrl: './register-page.component.css'
})
export class RegisterPageComponent {
  errorMessage: string = "";
  registerForm:FormGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    username: new FormControl('', [Validators.required, this.usernameValidator()]),
    location: new FormControl(),
    password: new FormControl('', [Validators.required, this.passwordValidator()]),
    confirmPassword: new FormControl('', [Validators.required], [this.confirmPasswordValidator()]),
  });

  constructor(private userService: UserService, private router: Router) {
  }

  submitForm(){
    this.errorMessage = '';
    if(this.registerForm.valid){
      const user:RegisterUserDto = {
        email: this.registerForm.controls['email'].value,
        password: this.registerForm.controls['password'].value,
        location: this.registerForm.controls['location'].value,
        username: this.registerForm.controls['username'].value
      }
      this.register(user);
    }
  }

  private register(user:RegisterUserDto){
    this.userService.registerUser(user).subscribe({
      next: () => {
        this.router.navigate(["/login"]);
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
  private usernameValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if(control.value.length < 4){
        return {minLength:{value:control.value}};
      }
      if(control.value.length > 20){
        return {maxLength:{value:control.value}};
      }
      return null;
    };
  }

  private passwordValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if(control.value.length < 8){
        return {minLength:{value:control.value}};
      }
      if(control.value.length > 20){
        return {maxLength:{value:control.value}};
      }
      const whiteSpaceRegex = new RegExp("^(?!.* ).{6,20}$")
      if(!whiteSpaceRegex.test(control.value)){
        return {whitespace:{value:control.value}};
      }
      return null;
    };
  }

  private confirmPasswordValidator(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      return of((() => {
        const passwordField = this.registerForm.controls['password'];
        if(passwordField?.valid){
          if(passwordField.value != control.value){
            return {passwordsNotMatching:{value:control.value}};
          }
        }
        return null;
      })());
    };
  }
}
