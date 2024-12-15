import { Component } from '@angular/core';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import {AuthService} from "../../../services/auth.service";
import {Router} from "@angular/router";
import {NgIf} from "@angular/common";

@Component({
  selector: 'app-toolbar',
  standalone: true,
  imports: [MaterialModule, NgIf],
  templateUrl: './toolbar.component.html',
  styleUrl: './toolbar.component.css'
})
export class ToolbarComponent {
  constructor(public authService: AuthService, private router: Router) {}

  logout(){
    this.authService.logout();
    this.router.navigate(["/"]);
  }
}
