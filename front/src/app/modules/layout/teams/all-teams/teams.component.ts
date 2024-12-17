import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { TeamsData } from 'app/models/models';
import { TeamService } from 'app/services/team.service';
import { CreateTeamDialogComponent } from '../create-team-dialog/create-team-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-teams',
  standalone: true,
  imports: [MaterialModule, FormsModule],
  templateUrl: './teams.component.html',
  styleUrl: './teams.component.css'
})
export class TeamsComponent {
  displayedColumns: string[] = ['position', 'name', 'description'];
  teams: TeamsData[] = [];

  constructor(private teamService: TeamService, private dialog: MatDialog) {
    this.getTeams("4e543f2d-799e-4538-a739-fbf5882761b6");
  }

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

  onRowClick(row: any) {
    console.log(row);
  }

  openCreateTeamForm(): void {
    const dialogRef = this.dialog.open(CreateTeamDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        console.log('Team created:', result);
      }
    });
  }

}
