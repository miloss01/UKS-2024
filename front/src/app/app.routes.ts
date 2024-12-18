import { Routes } from '@angular/router';
import { LandingPageComponent } from './modules/layout/landing-page/landing-page.component';
import {LoginPageComponent} from "./modules/layout/login-page/login-page.component";
import {HomePageComponent} from "./modules/layout/home-page/home-page.component";
import { TeamsComponent } from './modules/layout/teams/all-teams/teams.component';
import { TeamDetailsComponent } from './modules/layout/teams/team-details/team-details.component';

export const routes: Routes = [
  { path: 'login', component: LoginPageComponent },
  { path: 'home', component: HomePageComponent },
  { path: 'teams', component: TeamsComponent },
  { path: 'team-details/:id', component: TeamDetailsComponent },
  { path: '**', component: LandingPageComponent }
];
