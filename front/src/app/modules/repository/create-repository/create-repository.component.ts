import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BrowserAnimationsModule, NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { Repository } from 'app/models/models';

@Component({
  selector: 'app-create-repository',
  standalone: true,
  imports: [ MaterialModule ],
  templateUrl: './create-repository.component.html',
  styleUrl: './create-repository.component.css'
})
export class CreateRepositoryComponent {
  namespaces: string[] = ['namespace1', 'namespace2', 'namespace3'];
  repository: Repository = {
    name: '',
    namespace: '',
    description: '',
    visibility: 'public'
  }

  repositoryForm: FormGroup = new FormGroup({
    name: new FormControl(this.repository.name, [
      Validators.required,
      Validators.minLength(4),
      // forbiddenNameValidator(/bob/i), // <-- Here's how you pass in the custom validator.
    ]),
    namespace: new FormControl(this.repository.name, [
      Validators.required,
    ]),
    description: new FormControl(this.repository.description),
    visibility: new FormControl(this.repository.visibility, Validators.required),
  });
  

  constructor(private fb: FormBuilder) {
    // this.repositoryForm = this.fb.group({
    //   namespace: [this.repository.namespace, Validators.required],
    //   name: [this.repository.name, [Validators.required, Validators.minLength(3)]],
    //   description: [this.repository.description],
    //   visibility: [this.repository.visibility, Validators.required], // Default to "public"
    // });
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
