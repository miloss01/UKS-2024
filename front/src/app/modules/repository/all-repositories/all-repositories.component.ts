import { LiveAnnouncer } from '@angular/cdk/a11y';
import { AfterViewInit, Component, inject, Signal, signal, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { Repository } from 'app/models/models';

@Component({
  selector: 'app-all-repositories',
  standalone: true,
  imports: [ MaterialModule, FormsModule ],
  templateUrl: './all-repositories.component.html',
  styleUrl: './all-repositories.component.css'
})
export class AllRepositoriesComponent  implements AfterViewInit{
  namespaces: string[] = ["n1", "n2", "n3"]
  categories: string[] = ["c1", "c2", "c3"]
  searchQuery: Signal<string> = signal("");
  displayedColumns: string[] = ["name", "lastPushed", "contains", "visibility"]
  repositories: Repository[] = [
    {
      id: '1',
      images: [],
      lastPushed: '2023-11-01T12:00:00Z',
      createdAt: '2023-01-01T10:00:00Z',
      name: 'repository-one',
      owner: 'user1',
      description: 'This is the first repository.',
      isPublic: true
    },
    {
      id: '2',
      images: [{
        name: '',
        tags: [],
        pushed: ''
      }],
      lastPushed: '2023-11-05T08:30:00Z',
      createdAt: '2023-02-01T15:45:00Z',
      name: 'repository-two',
      owner: 'teamA',
      description: 'This is the second repository.',
      isPublic: false
    },
    {
      id: '3',
      images: [],
      lastPushed: '2023-10-20T14:15:00Z',
      createdAt: '2023-03-01T20:00:00Z',
      name: 'repository-three',
      owner: 'projectX',
      description: 'This is the third repository.',
      isPublic: true
    }
  ]
  
      
  repositorySource = new MatTableDataSource(this.repositories)

  router = inject(Router)
  private _liveAnnouncer = inject(LiveAnnouncer);

  @ViewChild(MatSort)
  sort: MatSort = new MatSort;

  @ViewChild(MatPaginator)
  paginator!: MatPaginator;


  ngAfterViewInit() {
    this.repositorySource.sort = this.sort;
    this.repositorySource.paginator = this.paginator
  }

  announceSortChange(sortState: Sort) {
    // This example uses English messages. If your application supports
    // multiple language, you would internationalize these strings.
    // Furthermore, you can customize the message to add additional
    // details about the values being sorted.
    if (sortState.direction) {
      this._liveAnnouncer.announce(`Sorted ${sortState.direction}ending`);
    } else {
      this._liveAnnouncer.announce('Sorting cleared');
    }
  }

  onCreate(): void {
    this.router.navigate(["/create-repo"])
    
  }

  openRepository(repository: Repository): void {
    console.log(repository)
    this.router.navigate(["/single-repo", repository.id])
  }
}
