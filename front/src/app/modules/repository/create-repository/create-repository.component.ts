import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import RepositoryCreation, { Repository } from 'app/models/models';
import { RepositoryService } from '../services/repository.service';
import { AuthService } from 'app/services/auth.service';

@Component({
  selector: 'app-create-repository',
  standalone: true,
  imports: [ MaterialModule, ReactiveFormsModule ],
  templateUrl: './create-repository.component.html',
  styleUrl: './create-repository.component.css'
})
export class CreateRepositoryComponent {
  namespaces: string[] = [];


  repoForm:FormGroup = new FormGroup({
    namespace: new FormControl("", [Validators.required]),
    description: new FormControl(''),
    visibility: new FormControl("", [Validators.required]),
    name: new FormControl("", [Validators.required])
  });

  constructor(
    private repositoryService: RepositoryService,
    private authService: AuthService
  ) {
    this.namespaces.push(this.authService.userData.value?.userEmail || "")

  }


  onCreate(): void {
      if (this.repoForm.valid) {
        console.log("AAAAAAAA")
        var repository: RepositoryCreation = {
          name: this.repoForm.controls['name'].value,
          owner: this.repoForm.controls['namespace'].value,
          description: this.repoForm.controls['description'].value,
          isPublic: this.repoForm.controls['visibility'].value === 'true'
        }
        console.log(repository)
        this.repositoryService.CreateRepository(repository).subscribe({
          next: (response: Repository) => {
            console.log('Repository created successfully:', response);
          },
          error: (error) => {
            console.error('Error creating repository:', error);
          }
        });    
      } else {
        console.error('Form is invalid');
      }
  }

  onCancel(): void {
    console.log('Repository creation canceled');
    // Add logic to navigate away or reset the form if needed
  }

}
