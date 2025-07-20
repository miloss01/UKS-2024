import { Component, Input, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { Member, TeamsData } from 'app/models/models';
import { TeamService } from 'app/services/team.service';
import { CreateTeamDialogComponent } from '../create-team-dialog/create-team-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-teams',
  standalone: true,
  imports: [CommonModule, MaterialModule, FormsModule],
  templateUrl: './teams.component.html',
  styleUrl: './teams.component.css'
})
export class TeamsComponent {
  @Input() organizationId : string | null = "";
  @Input() members : Member[] = [];
  @Input() isOwner : boolean | null = false;

  displayedColumns: string[] = ['position', 'name', 'description'];
  teams: TeamsData[] = [];

  constructor(private teamService: TeamService,
    private dialog: MatDialog,
    private router: Router) {}

  async getTeams(organizationId: string) {
    this.teamService.getTeams(organizationId).subscribe(
      (data: TeamsData[]) => {
        this.teams = data;
        this.teams.sort((a, b) => a.name.localeCompare(b.name));
        console.log('Teams loaded:', this.teams);
      },
      (error) => {
        console.error('Error fetching teams:', error);
      }
    );
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['organizationId'] && this.organizationId) {
      this.getTeams(this.organizationId);
    }
  }

  onRowClick(row: any) {
    this.router.navigate(['/team-details', row.id], {
      queryParams: { isOwner: this.isOwner }
    });
  }

  openCreateTeamForm(): void {
    const dialogRef = this.dialog.open(CreateTeamDialogComponent, {data: {members: this.members}});

    dialogRef.afterClosed().subscribe(result => {
      console.log("resulltttt " + result?.isClosed); 
      if (!result?.isClosed) {
        result.data.organizationId = this.organizationId;
        this.teamService.createTeam(result.data).subscribe((team) => {
          this.teams.push(team);
          this.teams = [...this.teams];  // update table
        });
        result.isClosed = true;
      }
    });
  }
}
