import { Routes } from '@angular/router';
import { LandingPageComponent } from './modules/layout/landing-page/landing-page.component';
import { CreateRepositoryComponent } from './modules/repository/create-repository/create-repository.component';
import {LoginPageComponent} from "./modules/layout/login-page/login-page.component";
import {HomePageComponent} from "./modules/layout/home-page/home-page.component";
import { SingeRepositoryComponent } from './modules/repository/singe-repository/singe-repository.component';

export const routes: Routes = [
  { path: 'single-repo', component: SingeRepositoryComponent },
  { path: 'create-repo', component: CreateRepositoryComponent },
  { path: 'login', component: LoginPageComponent },
  { path: 'home', component: HomePageComponent },
  { path: '**', component: LandingPageComponent }
];
