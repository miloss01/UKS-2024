import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { MinifiedStandardUserDTO, NewBadgeDTO } from 'app/models/models';
import { UserService } from 'app/services/user.service';

@Component({
  selector: 'app-user-badges',
  standalone: true,
  imports: [
    MaterialModule,
    CommonModule
  ],
  templateUrl: './user-badges.component.html',
  styleUrl: './user-badges.component.css'
})
export class UserBadgesComponent implements OnInit {

  users: MinifiedStandardUserDTO[] = [];

  displayedColumns: string[] = ['username', 'badge', 'actions'];
  badges: string[] = ['NoBadge', 'VerifiedPublisher', 'SponsoredOSS'];

  constructor(private userService: UserService, private snackBar: MatSnackBar) {}

  ngOnInit(): void {
    this.userService.getAllStandardUsers().subscribe({
      next: (res: MinifiedStandardUserDTO[]) => {
        this.users = res;
      },
      error: (err: any) => {
        console.log(err);
      }
    })
  }

  changeBadge(userId: string, newBadge: string): void {
    const newBadgeDTO: NewBadgeDTO = {
      badge: newBadge
    };

    this.userService.changeBadge(userId, newBadgeDTO).subscribe({
      next: () => {
        this.snackBar.open('Successfully changed badge.', 'Close', { duration: 3000 });
      },
      error: (err: any) => {
        console.log(err);
      }
    })
  }

}
