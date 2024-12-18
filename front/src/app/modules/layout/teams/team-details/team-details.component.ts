import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { TeamsData } from 'app/models/models';
import { TeamService } from 'app/services/team.service';
import { EditTeamDialogComponent } from '../edit-team-dialog/edit-team-dialog.component';
import { DeleteTeamDialogComponent } from '../delete-team-dialog/delete-team-dialog.component';

@Component({
  selector: 'app-team-details',
  standalone: true,
  imports: [MaterialModule, FormsModule, CommonModule],
  templateUrl: './team-details.component.html',
  styleUrl: './team-details.component.css'
})
export class TeamDetailsComponent implements OnInit {
  team: TeamsData | undefined;

  constructor(private route: ActivatedRoute, private teamService: TeamService, private dialog: MatDialog) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      console.log(id);
      this.teamService.getTeam(id).subscribe((team) => {
        this.team = team;
        console.log(team);
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
      }
    });
  }

  openDeleteDialog(): void {
    if (this.team != undefined) {
      const dialogRef = this.dialog.open(DeleteTeamDialogComponent, {
        width: '300px',
        data: { name: this.team.name },
      });

      dialogRef.afterClosed().subscribe((confirmed) => {
        if (confirmed) {
          console.log('Team deleted: ', this.team?.id);
          // TODO: call service method
        }
      });
    }
  }

}
