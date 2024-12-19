import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { CommonModule } from '@angular/common';
import { TeamService } from 'app/services/team.service';

@Component({
  selector: 'app-create-team-dialog',
  standalone: true,
  imports: [MaterialModule, ReactiveFormsModule, CommonModule],
  templateUrl: './create-team-dialog.component.html',
  styleUrl: './create-team-dialog.component.css'
})
export class CreateTeamDialogComponent {
  teamForm: FormGroup;
  members = [
    { email: 'user1@email.com' },
    { email: 'user2@email.com' },
  ]; 

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<CreateTeamDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.teamForm = this.fb.group({
      name: ['', [Validators.required]],
      description: [''],
      members: [[], [Validators.required, Validators.minLength(1)]], // at least one member selected
    });
  }

  onSubmit(): void {
    if (this.teamForm.valid) {
      this.dialogRef.close(this.teamForm.value); 
    }
  }

  onCancel(): void {
    this.dialogRef.close(); 
  }

}
