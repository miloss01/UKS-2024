import { LiveAnnouncer } from '@angular/cdk/a11y';
import { AfterViewInit, Component, inject, Signal, signal, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { DockerRepositoryDTO } from 'app/models/models';
import { RepositoryService } from '../services/repository.service';
import { AuthService } from 'app/services/auth.service';
import { OrganizationService } from 'app/services/organization.service';

@Component({
  selector: 'app-all-repositories',
  standalone: true,
  imports: [ MaterialModule, FormsModule ],
  templateUrl: './all-repositories.component.html',
  styleUrl: './all-repositories.component.css'
})
export class AllRepositoriesComponent  implements AfterViewInit{
  namespaces: string[] = []
  categories: string[] = ["c1", "c2", "c3"]
  selectedNamespace: string = "";
  searchQuery: string = "";
  displayedColumns: string[] = ["name", "lastPushed", "contains", "visibility"]
  repositories: DockerRepositoryDTO[] = []
  
      
  repositorySource = new MatTableDataSource<DockerRepositoryDTO>(this.repositories)

  router = inject(Router)
  private _liveAnnouncer = inject(LiveAnnouncer);
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private readonly repositoryService: RepositoryService,
              private readonly authService: AuthService,
              private readonly organizationService: OrganizationService
            ) 
  {
    this.fillNamespaces()
    const userId: string = this.authService.userData.value?.userId || ""
    this.repositoryService.GetUsersRepositories(userId).subscribe({
      next: (response: DockerRepositoryDTO[]) => {
        console.log(response)
        this.repositories = response
        this.repositorySource.data = response;

        this.repositorySource.filterPredicate = (data, filter) => {
          const searchTerms = JSON.parse(filter);
          const matchName = data.name.toLowerCase().includes(searchTerms.name);
          const matchNamespace =
            !searchTerms.namespace || data.owner.toLowerCase() === searchTerms.namespace;
          return matchName && matchNamespace;
        };

      },
      error: (error) => {
        console.error('Error creating repository:', error);
      }
    }); 

  }

  fillNamespaces() {
    console.log(this.authService.userData.value)
    const username: string = this.authService.userData.value?.username || ""
    const email: string = this.authService.userData.value?.userEmail || ""
    this.namespaces.push(username)
    this.organizationService.getOrganizations(email).subscribe({
      next: (data) => {
        console.log(data)
        data.forEach(organization => {
          this.namespaces.push(organization.name)          
        });
      },
      error: (err) => {
        console.error('Error fetching organizations:', err);
      }
    });

  }

  ngAfterViewInit() {
    this.repositorySource.sort = this.sort;
    this.repositorySource.paginator = this.paginator
  }

  announceSortChange(event: any) {
    console.log('Sort changed: ', event);
  }

  onCreate(): void {
    this.router.navigate(["/create-repo"])
    
  }

  openRepository(repository: DockerRepositoryDTO): void {
    console.log(repository)
    this.router.navigate(["/single-repo", repository.id])
  }

  applyFilters() {
    const filterValue = {
      name: this.searchQuery.trim().toLowerCase(),
      namespace: this.selectedNamespace.trim().toLowerCase(),
    };
    this.repositorySource.filter = JSON.stringify(filterValue);
  }
}
