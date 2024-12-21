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

  constructor(private dockerRepositoryService: RepositoryService, private authService: AuthService) {}

  ngOnInit(): void {
    if (this.authService.userData.value?.userRole == "StandardUser") {
      const userId: string = this.authService.userData.value.userId;
      this.dockerRepositoryService.getStarDockerRepositoriesForUser(userId).subscribe({
        next: (res: DockerRepositoryDTO[]) => {
          this.starRepositories = res;
        },
        error: (err: any) => {
          console.log(err);
        }
      })
    }
  }

}
