import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { FormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-edit-organization',
  standalone: true,
  imports: [ CommonModule, MaterialModule, FormsModule ],
  templateUrl: './edit-organization.component.html',
  styleUrl: './edit-organization.component.css'
})
export class EditOrganizationComponent {
  id: string;
  name: string;
  organizationDescription: string;
  imagePreview: string | null = null;
  isUploading = false;

  constructor(
    public dialogRef: MatDialogRef<EditOrganizationComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { orgId: string, name:string, desc: string; imageUrl: string }
  ) {
    this.id = data.orgId;
    this.name = data.name;
    this.organizationDescription = data.desc;
    this.imagePreview = data.imageUrl || null;
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input?.files && input.files[0]) {
      const reader = new FileReader();
      reader.onload = () => {
        this.imagePreview = reader.result as string;
      };
      reader.readAsDataURL(input.files[0]);
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onSave(): void {
    this.isUploading = true;
    // Simulacija slanja podataka
    setTimeout(() => {
      this.dialogRef.close({
        description: this.organizationDescription,
        imageUrl: this.imagePreview,
      });
    }, 2000);
  }
}
