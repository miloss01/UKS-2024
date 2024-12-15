import { Component, Input, SimpleChanges } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { Repository } from 'app/models/models';

@Component({
  selector: 'app-general-overview',
  standalone: true,
  imports: [ MaterialModule, ReactiveFormsModule ],
  templateUrl: './general-overview.component.html',
  styleUrl: './general-overview.component.css'
})
export class GeneralOverviewComponent {
  @Input() repository: Repository = {
    images: [],
    lastPushed: '',
    name: '',
    namespace: '',
    description: '',
    visibility: '',
    createdAt: '',
    id: 0
  }
  desctiptionEdditing: boolean = false
  descriptionFormControl = new FormControl('');

  // ngOnChanges(changes: SimpleChanges): void {
  //   if (changes['repository']) {
  //     this.descriptionFormControl.setValue(this.repository.description || '');
  //   }
  // }

  onDescriptionEddit(): void {
    console.log('edit');
    this.descriptionFormControl.setValue(this.repository.description);
    this.desctiptionEdditing = true
    // Add logic to navigate away or reset the form if needed
  }

  onDescriptionSave(): void {
    this.desctiptionEdditing = false
    this.repository.description = this.descriptionFormControl.value || ""
    // Send the change request
  }

  onDescriptionCancel(): void {
    this.desctiptionEdditing = false
  }
}
