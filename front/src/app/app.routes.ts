import { Routes } from '@angular/router';
import { LandingPageComponent } from './modules/layout/landing-page/landing-page.component';
import { CreateRepositoryComponent } from './modules/repository/create-repository/create-repository.component';
import {LoginPageComponent} from "./modules/layout/login-page/login-page.component";
import {HomePageComponent} from "./modules/layout/home-page/home-page.component";
import { SingeRepositoryComponent } from './modules/repository/singe-repository/singe-repository.component';
import { AllRepositoriesComponent } from './modules/repository/all-repositories/all-repositories.component';
import {ExplorePageComponent} from "./modules/layout/explore-page/explore-page.component";
import { PublicRepositoryOverviewComponent } from './modules/layout/public-repository-overview/public-repository-overview.component';

export const routes: Routes = [
  { path: 'all-user-repo', component: AllRepositoriesComponent },
  { path: 'single-repo/:id', component: SingeRepositoryComponent },
  { path: 'create-repo', component: CreateRepositoryComponent },
  { path: 'login', component: LoginPageComponent },
  { path: 'home', component: HomePageComponent },
  { path: 'explore', component: ExplorePageComponent },
  { path: 'explore/repository/:id', component: PublicRepositoryOverviewComponent },
  { path: '**', component: LandingPageComponent }
];
