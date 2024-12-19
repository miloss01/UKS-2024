import { Component } from '@angular/core';
import {ActivatedRoute, Router, RouterOutlet} from '@angular/router';
import { ToolbarComponent } from './modules/layout/toolbar/toolbar.component';
import {NgIf} from "@angular/common";


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, ToolbarComponent, NgIf],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'front';

  constructor() {
  }

}
