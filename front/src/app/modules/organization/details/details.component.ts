import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { FormsModule } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { PaginatorComponent } from '../paginator/paginator.component';

@Component({
  selector: 'app-details',
  standalone: true,
  imports: [CommonModule, FormsModule, MaterialModule, PaginatorComponent],
  templateUrl: './details.component.html',
  styleUrl: './details.component.css'
})
export class DetailsComponent implements OnInit {
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

  ngOnInit() {
    this.users = this.getAllUsers();
    this.filteredUsers = [...this.users];
    this.updateDisplayedUsers();

    this.members = this.getMembers();
    this.displayedMembers = [...this.members];
    this.updateDisplayedMembers();
  }

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
    // this.updateDisplayedMembers();
  }

  onPageSizeChangeMember(newSize: number) {
    // this.pageSizeMembers = newSize;
    this.currentPageMembers = 0; 
    // this.updateDisplayedMembers();
  }

  updateDisplayedMembers() {
    const startIndex = (this.currentPageMembers - 1) * this.pageSizeMembers;
    const endIndex = startIndex + this.pageSizeMembers;
    console.log("**************************")
    console.log(startIndex)
    console.log(endIndex)
    console.log("**************************")
    this.displayedMembers = this.members.slice(startIndex, endIndex);
  }

  addUser(user: any) {
    console.log('Adding user:', user);
  }
}
