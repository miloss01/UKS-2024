import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MaterialModule } from 'app/infrastructure/material/material.module';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [RouterLink, MaterialModule],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.css'
})
export class HomePageComponent {

}
