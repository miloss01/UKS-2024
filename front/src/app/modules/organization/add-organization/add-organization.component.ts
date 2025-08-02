import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { FormsModule } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OrganizationService } from 'app/services/organization.service';
import { AuthService } from 'app/services/auth.service';
import { ImageService } from 'app/services/image.service';

@Component({
  selector: 'app-add-organization',
  standalone: true,
  imports: [CommonModule, FormsModule, MaterialModule],
  templateUrl: './add-organization.component.html',
  styleUrl: './add-organization.component.css'
})
export class AddOrganizationComponent {
  organizationName: string = '';
  organizationDescription: string = '';
  imageFile: File | null = null;
  imagePreview: string | null = null;
  isUploading: boolean = false;

  constructor(private dialogRef: MatDialogRef<AddOrganizationComponent>, 
    private snackBar: MatSnackBar,
    private organizationService: OrganizationService,
    private imgService: ImageService,
    private authService: AuthService) 
  {}

  onFileSelected(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    if (inputElement.files && inputElement.files.length > 0) {
      this.imageFile = inputElement.files[0];

      const reader = new FileReader();
      reader.onload = (e) => {
        this.imagePreview = e.target?.result as string;
      };
      reader.readAsDataURL(this.imageFile);
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onSave(): void {
    const newOrganization = {
      name: this.organizationName,
      description: this.organizationDescription,
      imageLocation: this.imageFile?.name,
      ownerEmail: this.authService.userData?.value?.userEmail
    };

    this.isUploading = true; 

    this.organizationService.addOrganization(newOrganization).subscribe({
      next: (response) => {

         if(this.imagePreview != null && this.imageFile != null) 
          {
            let fileName = this.authService.userData?.value?.userEmail+"/"+response+"/"+this.imageFile.name
            this.imgService.uploadImage(fileName, this.imageFile).subscribe({
              next: (response) => {
                this.snackBar.open('Successfully created an organization', 'Close', { duration: 3000 });
                this.isUploading = false;
                this.dialogRef.close(true);
              },
              error: (error) => {
                console.error('Saving picture error!', error);
                this.isUploading = false; 
              }
            });
          }
      },
      error: (error) => {
        this.isUploading = false; 
        this.snackBar.open(error?.error?.message, 'Close', { duration: 3000 });
      }
    });
  }
}
