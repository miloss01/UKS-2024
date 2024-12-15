import { Component, EventEmitter, Inject, inject, Input, Output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { Repository } from 'app/models/models';

@Component({
  selector: 'app-change-visibility-popup',
  standalone: true,
  imports: [ MaterialModule, FormsModule ],
  templateUrl: './change-visibility-popup.component.html',
  styleUrl: './change-visibility-popup.component.css'
})
export class ChangeVisibilityPopupComponent {
  repository: Repository;

  // Inject MAT_DIALOG_DATA and MatDialogRef
  constructor(@Inject(MAT_DIALOG_DATA) private data: { repository: Repository }, private dialogRef: MatDialogRef<ChangeVisibilityPopupComponent>) {
    // Extract the repository from the data
    this.repository = data.repository;

  }

  userInput = signal("")
  onChange() {
    if (this.userInput() === this.repository.name){
      console.log("super")
    }
    else {
      console.log("nije super")
    }
  }
  
  onCancel() {
    console.log(this.repository)
    this.dialogRef.close();
  }

}
