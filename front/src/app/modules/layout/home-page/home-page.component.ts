import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterModule } from '@angular/router';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { DockerRepositoryDTO } from 'app/models/models';
import { AuthService } from 'app/services/auth.service';
import { RepositoryService } from 'app/services/repository.service';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [
    RouterModule,
    MaterialModule,
    CommonModule
  ],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.css'
})
export class HomePageComponent implements OnInit {

  starRepositories: DockerRepositoryDTO[] = [];
  privateRepositories: DockerRepositoryDTO[] = [];

  constructor(private dockerRepositoryService: RepositoryService, private authService: AuthService) {}

  ngOnInit(): void {
    const userId: string = this.authService.userData.value?.userId!;

    if (this.authService.userData.value?.userRole == "StandardUser") {
      this.dockerRepositoryService.getStarDockerRepositoriesForUser(userId).subscribe({
        next: (res: DockerRepositoryDTO[]) => {
          this.starRepositories = res;
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    }

    this.dockerRepositoryService.getPrivateDockerRepositoriesForUser(userId).subscribe({
      next: (res: DockerRepositoryDTO[]) => {
        this.privateRepositories = res;
      },
      error: (err: any) => {
        console.log(err);
      }
    })
  }

}
