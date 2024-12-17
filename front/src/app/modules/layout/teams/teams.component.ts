import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from 'app/infrastructure/material/material.module';

@Component({
  selector: 'app-teams',
  standalone: true,
  imports: [MaterialModule, FormsModule],
  templateUrl: './teams.component.html',
  styleUrl: './teams.component.css'
})
export class TeamsComponent {
  displayedColumns: string[] = ['position', 'name', 'description'];
  dataSource = [{"Name": "Team1", "Description": "This is some desc"}, {"Name": "Team2", "Description": "This is some desc"}];

  constructor() {

  }

  onRowClick(row: any) {
    console.log(row);
  }

}
