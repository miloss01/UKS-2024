import { Component, inject } from '@angular/core';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { GeneralOverviewComponent } from '../general-overview/general-overview.component';
import { TagsComponent } from '../tags/tags.component';
import { SettingsComponent } from '../settings/settings.component';
import { Repository } from 'app/models/models';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-singe-repository',
  standalone: true,
  imports: [ MaterialModule, GeneralOverviewComponent, TagsComponent, SettingsComponent ],
  templateUrl: './singe-repository.component.html',
  styleUrl: './singe-repository.component.css'
})
export class SingeRepositoryComponent {
  repository: Repository = {
    images: [],
    lastPushed: '12,23,32',
    name: 'lalala',
    owner: 'selena',
    description: 'fdvdvd',
    isPublic: true,
    createdAt: '1.1.235',
    id: 0
  }
  route = inject(ActivatedRoute);

  // bice da se ucita ovde
  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = params['id'];
      // this.repositoryService.getRepositoryById(id).subscribe(repo => {
      //   this.repository = repo; // Assign fetched repository details
      // })
      console.log(id)
    })
  }
}
