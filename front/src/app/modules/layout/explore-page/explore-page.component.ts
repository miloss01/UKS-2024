import { Component, OnInit } from '@angular/core';
import { DockerImageDTO, PageDTO } from 'app/models/models';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { DockerImageService } from 'app/services/docker-image.service';
import { RouterModule } from '@angular/router';

interface BadgeHelper {
  name: string;
  selected: boolean;
}

@Component({
  selector: 'app-explore-page',
  standalone: true,
  imports: [
    MaterialModule, 
    FormsModule, 
    CommonModule, 
    RouterModule
  ],
  templateUrl: './explore-page.component.html',
  styleUrl: './explore-page.component.css'
})
export class ExplorePageComponent implements OnInit {
  dockerImages: DockerImageDTO[] = [];
  badges: BadgeHelper[] = [
    { name: 'NoBadge', selected: false },
    { name: 'DockerOfficialImage', selected: false },
    { name: 'VerifiedPublisher', selected: false },
    { name: 'SponsoredOSS', selected: false },
  ];
  totalNumberOfElements: number = 0;
  page: number = 0;
  pageSize: number = 2;
  totalNumberOfPages: number = 0;
  searchTerm: string = '';

  oldSearchTerm: string = this.searchTerm;
  oldBadges: BadgeHelper[] = [];

  constructor(private dockerImageService: DockerImageService) {}

  ngOnInit(): void {
    this.applyFilters();
  }

  applyFilters(): void {
    this.page = 1;
    this.oldSearchTerm = this.searchTerm;
    this.oldBadges = this.badges.map(badge => ({ ...badge }));
    this.getDockerImages();
  }

  onPageChange(change: number): void {
    let newPage: number = this.page + change;

    if (newPage > this.totalNumberOfPages)
      newPage = 1;
    if (newPage < 1)
      newPage = this.totalNumberOfPages;

    this.page = newPage;

    this.getDockerImages();
  }

  onPageSizeChange(event: Event): void {
    const newPageSize = (event.target as HTMLSelectElement).value;
    this.pageSize = Number(newPageSize);
    this.page = 1;
    this.getDockerImages();
  }

  getDockerImages(): void {
    this.dockerImageService.getDockerImages(
      this.page, 
      this.pageSize, 
      this.oldSearchTerm, 
      this.getSelectedBadges(this.oldBadges)
    ).subscribe({
      next: (res: PageDTO<DockerImageDTO>) => {
        this.dockerImages = res.data;
        this.totalNumberOfElements = res.totalNumberOfElements;
        this.totalNumberOfPages = Math.ceil(this.totalNumberOfElements / this.pageSize);
        this.page = this.totalNumberOfElements != 0 ? this.page : 0
      },
      error: (err: any) => {
        console.log(err);
      }
    });
  }

  getSelectedBadges(badges: BadgeHelper[]): string {
    return badges
      .filter(badge => badge.selected)
      .map(badge => badge.name)
      .join(',')
  }
}
