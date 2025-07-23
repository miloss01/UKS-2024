import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MaterialModule } from 'app/infrastructure/material/material.module';

@Component({
  selector: 'app-add-repository-dialog',
  standalone: true,
  imports: [MaterialModule, FormsModule, CommonModule, ReactiveFormsModule],
  templateUrl: './add-repository-dialog.component.html',
  styleUrl: './add-repository-dialog.component.css'
})
export class AddRepositoryDialogComponent {

  repoForm: FormGroup;
  availableRepositories: string[] = ['Repo A', 'Repo B', 'Repo C']; // pass it as @Input()

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<AddRepositoryDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.availableRepositories = data?.availableRepositories || this.availableRepositories;

    this.repoForm = this.fb.group({
      repositoryName: ['', Validators.required],
      permission: [null, Validators.required]
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    if (this.repoForm.valid) {
      this.dialogRef.close(this.repoForm.value);
    }
  }
}
