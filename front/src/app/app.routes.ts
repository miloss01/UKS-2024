import { Routes } from '@angular/router';
import { LandingPageComponent } from './modules/layout/landing-page/landing-page.component';
import {LoginPageComponent} from "./modules/layout/login-page/login-page.component";

export const routes: Routes = [
  { path: 'login', component: LoginPageComponent },
  { path: '**', component: LandingPageComponent }
];
