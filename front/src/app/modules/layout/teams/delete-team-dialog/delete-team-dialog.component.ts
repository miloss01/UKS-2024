import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MaterialModule } from 'app/infrastructure/material/material.module';

@Component({
  selector: 'app-delete-team-dialog',
  standalone: true,
  imports: [MaterialModule],
  templateUrl: './delete-team-dialog.component.html',
  styleUrl: './delete-team-dialog.component.css'
})
export class DeleteTeamDialogComponent {
  constructor(
    private dialogRef: MatDialogRef<DeleteTeamDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { name: string }
  ) {}

  onDelete(): void {
    this.dialogRef.close(true); // Close the dialog and return "true" as confirmation
  }

}
