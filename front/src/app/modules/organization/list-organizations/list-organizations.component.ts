import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-list-organizations',
  standalone: true,
  imports: [CommonModule, FormsModule, MaterialModule],
  templateUrl: './list-organizations.component.html',
  styleUrl: './list-organizations.component.css'
})
export class ListOrganizationsComponent implements OnInit {
  searchQuery: string = '';

  extensions = [
    {
      name: 'Harpoon',
      publisher: 'Harpoon Corp',
      updated: '2 weeks ago',
      description: 'Docker Extension for the No Code Kubernetes platform.',
      downloads: 19.0,
      icon: 'assets/harpoon-icon.png'
    },
    {
      name: 'Grafana',
      publisher: 'Grafana Labs',
      updated: '1 month ago',
      description: 'Monitor your Docker Desktop instance from Grafana cloud.',
      downloads: 19.0,
      icon: 'assets/grafana-icon.png'
    },
    {
      name: 'Tailscale',
      publisher: 'Tailscale Inc.',
      updated: '1 month ago',
      description: 'Securely connect to Docker containers without exposing them.',
      downloads: 10.0,
      icon: 'assets/tailscale-icon.png'
    },
    {
      name: 'LAAAAAA',
      publisher: 'Tailscale Inc.',
      updated: '1 month ago',
      description: 'Securely connect to Docker containers without exposing them.',
      downloads: 10.0,
      icon: 'assets/tailscale-icon.png'
    }
  ];

  filteredExtensions = this.extensions;

  currentPage = 1; // current page
  pageSize = 2; // number items per page
  totalPages = 1; // num pages
  
  ngOnInit() {
    this.updatePagination();
  }

  onCreateRepository() {
    console.log('Create repository button clicked');
  }

  onSearchRepository() {
    this.filteredExtensions = this.extensions.filter(extension =>
      extension.name.toLowerCase().includes(this.searchQuery?.toLowerCase() || '')
    );
    this.currentPage = 1; 
    this.updatePagination();
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.filteredExtensions.length / this.pageSize);
  }

  updateTotalPages() {
    this.totalPages = Math.ceil(this.extensions.length / this.pageSize);
  }

  get paginatedExtensions() {
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    return this.extensions.slice(start, end);
  }

  changePage(step: number) {
    const newPage = this.currentPage + step;
    if (newPage > 0 && newPage <= this.totalPages) {
      this.currentPage = newPage;
    }
  }

  changePageSize(event: Event) {
    const newSize = (event.target as HTMLSelectElement).value; // Kastovanje
    this.pageSize = +newSize; 
    this.currentPage = 1; 
    this.updateTotalPages();
  }  
}
