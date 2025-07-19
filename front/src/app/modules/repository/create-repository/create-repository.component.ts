import { Component, inject, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import RepositoryCreation, { DockerRepositoryDTO } from 'app/models/models';
import { RepositoryService } from '../services/repository.service';
import { AuthService } from 'app/services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { OrganizationService } from 'app/services/organization.service';

@Component({
  selector: 'app-create-repository',
  standalone: true,
  imports: [ MaterialModule, ReactiveFormsModule ],
  templateUrl: './create-repository.component.html',
  styleUrl: './create-repository.component.css'
})
export class CreateRepositoryComponent implements OnInit {
  namespaces: {id:string, name:string}[] = [];
  orgName!: string | null;
  orgId!: string | null;

  router = inject(Router)

  repoForm:FormGroup = new FormGroup({
    namespace: new FormControl("", [Validators.required]),
    description: new FormControl(''),
    visibility: new FormControl('true', [Validators.required]),
    name: new FormControl("", [Validators.required])
  });

  constructor(
    private repositoryService: RepositoryService,
    private authService: AuthService,
    private organizationService: OrganizationService,
    private route: ActivatedRoute
  ) {
  }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.orgName = params['name'];
      this.orgId = params['id'];
    });
    this.fillNamespaces();
  }

  fillNamespaces() {
    if(this.orgId && this.orgName) {
      this.namespaces.push({
        id: this.orgId,
        name: this.orgName
      });
      this.repoForm.controls['namespace'].setValue(this.orgId);
      return;
    }
    const userEmail: string = this.authService.userData.value?.userEmail || ""
    const userId: string = this.authService.userData.value?.userId || ""
    this.namespaces.push({
      id: userId,
      name: userEmail
    })
    this.repoForm.controls['namespace'].setValue(userId)
    this.organizationService.getOrganizations(userEmail).subscribe({
      next: (data) => {
        console.log(data)
        data.forEach(organization => {
          console.log(organization)
          return this.namespaces.push({
            id: organization.id,
            name: organization.name
          });          
        });
      },
      error: (err) => {
        console.error('Error fetching organizations:', err);
      }
    });
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
          next: (response: DockerRepositoryDTO) => {
            console.log('Repository created successfully:', response);
            this.router.navigate(["/all-user-repo"])
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
