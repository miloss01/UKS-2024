import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { TeamsData } from 'app/models/models';
import { TeamService } from 'app/services/team.service';

@Component({
  selector: 'app-team-details',
  standalone: true,
  imports: [MaterialModule, FormsModule, CommonModule],
  templateUrl: './team-details.component.html',
  styleUrl: './team-details.component.css'
})
export class TeamDetailsComponent implements OnInit {
  team: TeamsData | undefined;

  constructor(private route: ActivatedRoute, private teamService: TeamService) {}

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

}
