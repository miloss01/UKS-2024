import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { FormsModule } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OrganizationService } from 'app/services/organization.service';

@Component({
  selector: 'app-add-organization',
  standalone: true,
  imports: [CommonModule, FormsModule, MaterialModule],
  templateUrl: './add-organization.component.html',
  styleUrl: './add-organization.component.css'
})
export class AddOrganizationComponent {
  imagePreview: string | null = null; // URL za pregled slike

  constructor(private dialogRef: MatDialogRef<AddOrganizationComponent>, 
    private snackBar: MatSnackBar,
    private organizationService: OrganizationService) 
  {}

  // Funkcija za upload slike
  onFileSelected(event: Event): void {
    const fileInput = event.target as HTMLInputElement;

    if (fileInput.files && fileInput.files[0]) {
      const file = fileInput.files[0];

      // Kreiranje URL za pregled slike
      const reader = new FileReader();
      reader.onload = () => {
        this.imagePreview = reader.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onSave(): void {
    const newOrganization = {
      name: "New Organization",
      description: "This is a new organization.",
      image: ""
    };
  
    this.organizationService.addOrganization(newOrganization).subscribe({
      next: (response) => {
        console.log('Organizacija sačuvana!', response);
        this.snackBar.open('Organizacija je uspešno sačuvana!', 'Zatvori', { duration: 3000 });
        this.dialogRef.close(true);
      },
      error: (error) => {
        console.error('Greška prilikom čuvanja organizacije!', error);
        this.snackBar.open('Došlo je do greške prilikom čuvanja organizacije!', 'Zatvori', { duration: 3000 });
        this.dialogRef.close(false);
      }
    });
  }
}
