import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { PaginatorComponent } from '../paginator/paginator.component';
import { OrganizationService } from 'app/services/organization.service';

@Component({
  selector: 'app-details',
  standalone: true,
  imports: [CommonModule, FormsModule, MaterialModule, PaginatorComponent],
  templateUrl: './details.component.html',
  styleUrl: './details.component.css'
})
export class DetailsComponent implements OnInit {
  id: string | null = null;
  name: string | null = null;
  isOwner: boolean | null = null;
  organization: any | null = null;

  users: any[] = [];
  filteredUsers: any[] = [];
  displayedAllUsers: any[] = [];
  members: any[] = [];
  displayedMembers: any[] = [];

  searchQuery = ''
  pageSize: number = 2;
  currentPage: number = 1;

  pageSizeMembers: number = 2;
  currentPageMembers: number = 1;

  constructor(private route: ActivatedRoute, private location: Location, private orgService: OrganizationService) {
  }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');
    console.log('Dobijeni GUID iz URL-a:', this.id);

    this.route.queryParams.subscribe(params => {
      this.name = params['name'];
    });

    console.log('ID:', this.id);
    console.log('Name:', this.name);

    // if(this.id != null)
    //   this.fetchOrganization(this.id);

    this.users = this.getAllUsers();
    this.filteredUsers = [...this.users];
    this.updateDisplayedUsers();

    if(this.id != null)
      this.fetchMembers(this.id)
    // this.members = this.getMembers();
  }

  fetchMembers(id: string): void {
    this.orgService.getMembersByOrganizationId(id).subscribe({
      next: (data) => {
        this.members = data;
        this.displayedMembers = [...this.members];
        this.updateDisplayedMembers();
        console.log(data)
        console.log("ok")
      },
      error: (err) => {
        this.members = [];
        console.log(err)
      }
    });
  }

  // fetchOrganization(id: string): void {
  //   this.orgService.getOrganizationById(id).subscribe({
  //     next: (data) => {
  //       this.organization = data;
  //       console.log(data)
  //       console.log("ok")
  //     },
  //     error: (err) => {
  //       this.organization = null;
  //       console.log(err)
  //     }
  //   });
  // }

  getAllUsers() {
    return [
        { firstName: 'Marko', lastName: 'Marković', email: 'marko@example.com', image: 'images/user.png' },
    { firstName: 'Ivana', lastName: 'Ivić', email: 'ivana@example.com', image: 'images/user.png' },
    { firstName: 'Marko', lastName: 'Marković', email: 'marko@example.com', image: 'images/user.png' },
    { firstName: 'Ivana', lastName: 'Ivić', email: 'ivana@example.com', image: 'images/user.png' },
    { firstName: 'Marko', lastName: 'Marković', email: 'marko@example.com', image: 'images/user.png' },
    { firstName: 'Ivana', lastName: 'Ivić', email: 'ivana@example.com', image: 'images/user.png' },
    ];
  }

  getMembers() {
    return [
      { firstName: 'Petar', lastName: 'Petrović', email: 'petar@example.com' },
      { firstName: 'Ana', lastName: 'Anić', email: 'ana@example.com' },
      { firstName: 'Ana', lastName: 'Anić', email: 'ana@example.com' },
      { firstName: 'Ana', lastName: 'Anić', email: 'ana@example.com' },
    ]
  }

  updateSearch() {
    this.filteredUsers = this.users.filter((user) => {
      const fullName = `${user.firstName} ${user.lastName}`.toLowerCase();
      const email = user.email.toLowerCase();
      const query = this.searchQuery.toLowerCase();

      return fullName.includes(query) || email.includes(query);
    });

    this.currentPage = 1;
    this.updateDisplayedUsers();
  }

  onPageChange(newPage: number) {
    this.currentPage = newPage;
    this.updateDisplayedUsers();
  }

  onPageSizeChange(newSize: number) {
    this.pageSize = newSize;
    this.currentPage = 1; 
    this.updateDisplayedUsers();
  }

  updateDisplayedUsers() {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.displayedAllUsers = this.filteredUsers.slice(startIndex, endIndex);
  }

  onPageChangeMember(newPage: number) {
    this.currentPageMembers = newPage;
    this.updateDisplayedMembers();
  }

  onPageSizeChangeMember(newSize: number) {
    this.pageSizeMembers = newSize;
    this.currentPageMembers = 1; 
    this.updateDisplayedMembers();
  }

  updateDisplayedMembers() {
    const startIndex = (this.currentPageMembers - 1) * this.pageSizeMembers;
    const endIndex = startIndex + this.pageSizeMembers;
    this.displayedMembers = this.members.slice(startIndex, endIndex);
  }

  addUser(user: any) {
    console.log('Adding user:', user);
  }

  goBack(): void {
    this.location.back(); // Vraća na prethodnu stranicu
  }
}
