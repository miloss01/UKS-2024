import { Routes } from '@angular/router';
import { LandingPageComponent } from './modules/layout/landing-page/landing-page.component';
import { CreateRepositoryComponent } from './modules/repository/create-repository/create-repository.component';

export const routes: Routes = [
  { path: '**', component: CreateRepositoryComponent },
  // { path: '**', component: LandingPageComponent }
];
