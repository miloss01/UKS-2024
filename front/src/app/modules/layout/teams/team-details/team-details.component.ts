import { CommonModule, Location } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { TeamsData } from 'app/models/models';
import { TeamService } from 'app/services/team.service';
import { EditTeamDialogComponent } from '../edit-team-dialog/edit-team-dialog.component';
import { DeleteTeamDialogComponent } from '../delete-team-dialog/delete-team-dialog.component';
import { AddRepositoryDialogComponent } from '../add-repository-dialog/add-repository-dialog.component';
import { RepositoryService } from 'app/services/repository.service';

@Component({
  selector: 'app-team-details',
  standalone: true,
  imports: [MaterialModule, FormsModule, CommonModule],
  templateUrl: './team-details.component.html',
  styleUrl: './team-details.component.css'
})
export class TeamDetailsComponent implements OnInit {
  isOwner: boolean | null = false;
  displayedColumns: string[] = ['name', 'visibility', 'permissions'];
  repositories: any[] = [];
  team: TeamsData | undefined;
  mapPermission : Record<string, string>  = {'0' : "ReadOnly", '1': "ReadWrite", '2': "Admin"}

  constructor(
    private route: ActivatedRoute,
    private teamService: TeamService,
    private repositoryService: RepositoryService,
    private dialog: MatDialog,
    private location: Location) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.isOwner = params['isOwner'] === 'true';
    });

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.teamService.getTeam(id).subscribe((team) => {
        this.team = team;
      });

      this.teamService.getRepositories(id).subscribe((repos) => {
        const newRepos = repos.map(repo => ({
          name: repo.repository.name, 
          visibility: repo.repository.isPublic, 
          permissions: repo.permission
        }));
        this.repositories = [...this.repositories, ...newRepos];
      });
    }
  }

  openEditDialog(): void {
    const dialogRef = this.dialog.open(EditTeamDialogComponent, {
      width: '400px',
      data: { ...this.team},
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result && this.team != undefined) {
        this.team.name = result.name;
        this.team.description = result.description;
        this.teamService.update(this.team).subscribe((team) => {
        });
      }
    });
  }

  openDeleteDialog(): void {
    if (this.team != undefined) {
      let id = this.team.id; // because there will be errors withoud this (team could be undefiend)
      const dialogRef = this.dialog.open(DeleteTeamDialogComponent, {
        width: '400px',
        data: { name: this.team.name},
      });

      dialogRef.afterClosed().subscribe((confirmed) => {
        if (confirmed) {
          this.teamService.deleteTeam(id).subscribe((res) => {
            if (res != null) {
              this.goBack();
            }
          });
        }
      });
    }
  }

  async openAddRepositoryDialog(): Promise<void> {
    if (this.team != undefined) {
      this.repositoryService.getByOrganizationId(this.team.organizationId).subscribe((repos) => {
          console.log(repos);
          const dialogRef = this.dialog.open(AddRepositoryDialogComponent, {
          width: '400px',
          data: {
            availableRepositories: repos // TODO: get available repositories here
          }
          });
          dialogRef.afterClosed().subscribe(result => {
          if (result) {
            console.log('Repository added:', result); // TODO: add respositories here (add on confirm check)
            var permission = {"teamId": this.team?.id, "repositoryId": result.repository.id, "permission": this.mapPermission[result.permission?.toString()]};
            console.log(permission);
            this.teamService.addPermission(permission).subscribe((_) => {
              this.teamService.getRepositories(this.team!.id).subscribe((repos) => {
                const newRepos = repos.map(repo => ({
                  name: repo.repository.name, 
                  visibility: repo.repository.isPublic, 
                  permissions: repo.permission
                }));
                this.repositories = [...newRepos];
              });
            });
          }
        });

      });
      
    } 
  }
  
  goBack(): void {
    this.location.back(); 
  }
}
