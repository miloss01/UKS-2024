import { Routes } from '@angular/router';
import { LandingPageComponent } from './modules/layout/landing-page/landing-page.component';
import {LoginPageComponent} from "./modules/layout/login-page/login-page.component";
import {HomePageComponent} from "./modules/layout/home-page/home-page.component";

export const routes: Routes = [
  { path: 'login', component: LoginPageComponent },
  { path: 'home', component: HomePageComponent },
  { path: '**', component: LandingPageComponent }
];
