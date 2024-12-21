import { Routes } from '@angular/router';
import { LandingPageComponent } from './modules/layout/landing-page/landing-page.component';
import { CreateRepositoryComponent } from './modules/repository/create-repository/create-repository.component';
import {LoginPageComponent} from "./modules/layout/login-page/login-page.component";
import {HomePageComponent} from "./modules/layout/home-page/home-page.component";
import { SingeRepositoryComponent } from './modules/repository/singe-repository/singe-repository.component';
import { AllRepositoriesComponent } from './modules/repository/all-repositories/all-repositories.component';
import {ChangePasswordPageComponent} from "./modules/layout/change-password-page/change-password-page.component";
import {ExplorePageComponent} from "./modules/layout/explore-page/explore-page.component";
import { PublicRepositoryOverviewComponent } from './modules/layout/public-repository-overview/public-repository-overview.component';
import {RegisterPageComponent} from "./modules/layout/register-page/register-page.component";
import { ListOrganizationsComponent } from './modules/organization/list-organizations/list-organizations.component';
import { DetailsComponent } from './modules/organization/details/details.component';
import { TeamsComponent } from './modules/layout/teams/all-teams/teams.component';
import { TeamDetailsComponent } from './modules/layout/teams/team-details/team-details.component';

export const routes: Routes = [
  { path: 'all-user-repo', component: AllRepositoriesComponent },
  { path: 'single-repo/:id', component: SingeRepositoryComponent },
  { path: 'create-repo', component: CreateRepositoryComponent },
  { path: 'login', component: LoginPageComponent },
  { path: 'home', component: HomePageComponent },
  { path: 'password/change', component: ChangePasswordPageComponent },
  { path: 'explore', component: ExplorePageComponent },
  { path: 'explore/repository/:id', component: PublicRepositoryOverviewComponent },
  { path: 'sign-up', component: RegisterPageComponent },
  { path: 'organizations', component: ListOrganizationsComponent },
  { path: 'org-details/:id', component: DetailsComponent },
  { path: 'teams', component: TeamsComponent },
  { path: 'team-details/:id', component: TeamDetailsComponent },
  { path: '**', component: LandingPageComponent }
];
