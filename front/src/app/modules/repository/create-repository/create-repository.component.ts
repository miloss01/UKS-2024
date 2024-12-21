import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import RepositoryCreation from 'app/models/models';

@Component({
  selector: 'app-create-repository',
  standalone: true,
  imports: [ MaterialModule ],
  templateUrl: './create-repository.component.html',
  styleUrl: './create-repository.component.css'
})
export class CreateRepositoryComponent {
  namespaces: string[] = ['namespace1', 'namespace2', 'namespace3'];
  repository: RepositoryCreation = {
    name: "sss",
    owner: 'dd',
    description: 'ddd',
    isPublic: true
  }
  repositoryForm: FormGroup<{ namespace: FormControl<string | null>; name: FormControl<string | null>; description: FormControl<string | null>; visibility: FormControl<boolean | null>; }>;

  constructor(private fb: FormBuilder,
    // private repositoryService: RepositoryService
  ) {
    this.repositoryForm = this.fb.group({
      namespace: [this.repository.owner, Validators.required],
      name: [this.repository.name, [Validators.required]],
      description: [this.repository.description],
      visibility: [this.repository.isPublic, Validators.required], // Default to "public"
    });
  }


  onCreate(): void {
    console.log('Namespace:', this.repositoryForm.get('namespace')?.value);
    console.log('Name:', this.repositoryForm.get('name')?.value);
    console.log('Description:', this.repositoryForm.get('description')?.value);
    console.log('Visibility:', this.repositoryForm.get('visibility')?.value);
    console.log(this.repository)
      if (this.repositoryForm.valid) {
      const formData = this.repositoryForm.value;
      console.log('Repository Created:', formData);
      // Add logic to call your backend API to create the repository
    } else {
      console.error('Form is invalid');
    }
  }

  onCancel(): void {
    console.log('Repository creation canceled');
    // Add logic to navigate away or reset the form if needed
  }

}
