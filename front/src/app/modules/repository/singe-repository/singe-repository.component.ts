import { Component, inject } from '@angular/core';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { GeneralOverviewComponent } from '../general-overview/general-overview.component';
import { SettingsComponent } from '../settings/settings.component';
import { DockerRepositoryDTO } from 'app/models/models';
import { ActivatedRoute } from '@angular/router';
import { ImagesListComponent } from 'app/modules/layout/images-list/images-list.component';
import { RepositoryService } from 'app/services/repository.service';
import { AuthService } from 'app/services/auth.service';

@Component({
  selector: 'app-singe-repository',
  standalone: true,
  imports: [ MaterialModule, GeneralOverviewComponent, ImagesListComponent, SettingsComponent ],
  templateUrl: './singe-repository.component.html',
  styleUrl: './singe-repository.component.css'
})
export class SingeRepositoryComponent {
  repository: DockerRepositoryDTO = {
    images: [],
    lastPushed: '12,23,32',
    name: 'lalala',
    owner: 'selena',
    description: 'fdvdvd',
    isPublic: true,
    createdAt: '1.1.235',
    id: "0",
    starCount: 0,
    badge: ''
  }
  permisionType: number = 0;
  route = inject(ActivatedRoute);

  constructor(private readonly repositoryService: RepositoryService,
                  private readonly authService: AuthService,){}

  ngOnInit(): void {
    this.getRepository();
    this.getPermisionType();
  }

  private getPermisionType() {
    this.route.params.subscribe(params => {
      const repoId = params['id'];
      const userId: string = this.authService.userData.value?.userId || "";
      this.repositoryService.GetUsersPermisionForRepository(userId, repoId).subscribe(permision => {
        this.permisionType = permision; 
        this.handlePermissionWhenUserIsTheOwner()
        console.log(this.permisionType)
      });
    });
  }

  private handlePermissionWhenUserIsTheOwner() {
    const userEmail: string = this.authService.userData.value?.userEmail || "";
    if (this.repository.owner == userEmail) {
      this.permisionType = 2
    }
  }

  private getRepository() {
    this.route.params.subscribe(params => {
      const id = params['id'];
      this.repositoryService.getDockerRepositoryById(id).subscribe(repo => {
        this.repository = repo; // Assign fetched repository details
        this.handlePermissionWhenUserIsTheOwner()
      });
      console.log(id);
    });
  }
}
