import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { AddOrganizationComponent } from '../add-organization/add-organization.component';
import { AuthService } from 'app/services/auth.service';
import { OrganizationService } from 'app/services/organization.service';
import { ImageService } from 'app/services/image.service';

@Component({
  selector: 'app-list-organizations',
  standalone: true,
  imports: [CommonModule, FormsModule, MaterialModule, RouterModule],
  templateUrl: './list-organizations.component.html',
  styleUrl: './list-organizations.component.css'
})
export class ListOrganizationsComponent implements OnInit {
  searchQuery: string = '';
  organizations: any[] = []; 
  filteredOrganizations: any[] = []; 

  currentPage = 1; // current page
  pageSize = 2; // number items per page
  totalPages = 1; // num pages

  constructor(private router: Router, private dialog: MatDialog, private authService: AuthService, private orgService: OrganizationService, private imageService: ImageService) 
  {}
  
  ngOnInit() {
    console.log(this.authService.userData?.userEmail)
    this.fetchUserOrganizations();
  }

  setImages() {
    this.organizations.forEach((org) => {
      console.log(org)
      console.log(this.authService.userData?.userEmail+"/"+org.Id+"/"+org.imageLocation)
      this.imageService.getImageUrl(this.authService.userData?.userEmail+"/"+org.id+"/"+org.imageLocation).subscribe({
        next: (response) => {
          org.imageUrl = response.imageUrl; 
        },
        error: (error) => {
          console.error('Error fetching image URL:', error);
        },
      });
    });
  }

  fetchUserOrganizations() {
    const email = this.authService.userData?.userEmail; 

    if (email) {
      this.orgService.getOrganizations(email).subscribe({
        next: (data) => {
          this.organizations = data.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());;  
          console.log(data)
          this.filteredOrganizations = data;  
          this.updatePagination();
          this.setImages()
        },
        error: (err) => {
          console.error('Error fetching organizations:', err);
        }
      });
    }
  }

  onSearchOrganization() {
    this.filteredOrganizations = this.organizations.filter(extension =>
      extension.name.toLowerCase().includes(this.searchQuery?.toLowerCase() || '')
    );
    this.currentPage = 1; 
    this.updatePagination();
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.filteredOrganizations.length / this.pageSize);
  }

  updateTotalPages() {
    this.totalPages = Math.ceil(this.organizations.length / this.pageSize);
  }

  get paginatedExtensions() {
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    return this.filteredOrganizations.slice(start, end);
  }  

  changePage(step: number) {
    const newPage = this.currentPage + step;
    if (newPage > 0 && newPage <= this.totalPages) {
      this.currentPage = newPage;
    }
  }

  changePageSize(event: Event) {
    const newSize = (event.target as HTMLSelectElement).value;
    this.pageSize = +newSize; 
    this.currentPage = 1; 
    this.updateTotalPages();
  }  

  goToDetails(id: number, name:string, isOwner: boolean): void {
    this.router.navigate(['/org-details', id], {
      queryParams: { name: name }
    });
  }

  openDialog() {
    const dialogRef = this.dialog.open(AddOrganizationComponent, {
      width: '400px', 
    });

    dialogRef.afterClosed().subscribe(result => {
      this.fetchUserOrganizations();
      // location.reload();
    });
  }
}
