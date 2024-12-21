import { Component, Inject, signal } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Repository } from 'app/models/models';
import { ChangeVisibilityPopupComponent } from '../change-visibility-popup/change-visibility-popup.component';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { FormsModule } from '@angular/forms';
import { RepositoryService } from '../services/repository.service';

@Component({
  selector: 'app-delete-repository-popup',
  standalone: true,
  imports: [MaterialModule, FormsModule ],
  templateUrl: './delete-repository-popup.component.html',
  styleUrl: './delete-repository-popup.component.css'
})
export class DeleteRepositoryPopupComponent {
  repository: Repository;
  userInput = signal("")

  // Inject MAT_DIALOG_DATA and MatDialogRef
  constructor(@Inject(MAT_DIALOG_DATA) private data: { repository: Repository }, 
  private dialogRef: MatDialogRef<ChangeVisibilityPopupComponent>,
  private readonly repositoryService: RepositoryService)
  {
    // Extract the repository from the data
    this.repository = data.repository;

  }

  onDelete() {
    if (this.userInput() === this.repository.name){
      console.log("super")
      if (this.userInput() === this.repository.name){
        console.log("super")
        this.repositoryService.DeleteRepository(this.repository.id).subscribe({
          next: (response: Repository) => {
            console.log('Deleted:', response);
            this.dialogRef.close();
  
          },
          error: (error) => {
            console.error('Error creating repository:', error);
          }
        }); 
      }
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
