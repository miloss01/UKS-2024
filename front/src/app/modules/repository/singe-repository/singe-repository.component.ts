import { Component } from '@angular/core';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { GeneralOverviewComponent } from '../general-overview/general-overview.component';
import { TagsComponent } from '../tags/tags.component';
import { SettingsComponent } from '../settings/settings.component';
import { Repository } from 'app/models/models';

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
    namespace: 'selena',
    description: 'fdvdvd',
    visibility: 'public',
    createdAt: '1.1.235',
    id: 0
  }

  // bice da se ucita ovde
}
