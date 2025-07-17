import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { LogsService } from 'app/services/logs.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-logs',
  standalone: true,
  imports: [MaterialModule, FormsModule, CommonModule, ReactiveFormsModule, MatSelectModule],
  templateUrl: './logs.component.html',
  styleUrl: './logs.component.css'
})
export class LogsComponent {
  query: string = ''; 
  startDate: Date | null = null; 
  startTime: string = '';
  endDate: Date | null = null;   
  endTime: string = '';
  logs: any[] = [];
  options = [
    { label: 'Select level', value: '' },
    { label: 'Info', value: 'inf' },
    { label: 'Warning', value: 'wrn' },
    { label: 'Error', value: 'err' },
  ];
  selectedOption: { label: string, value: string } = this.options[0];

  displayedColumns: string[] = ['timestamp', 'level', 'message']; 

  constructor(private logService: LogsService, private snackBar: MatSnackBar) {}

  ngOnInit(): void {
    this.loadInitialLogs();
  }

  loadInitialLogs(): void {
    this.logService.searchLogs({ query: null, level: null, startDate: null, endDate: null }).subscribe({
      next: (response: any[]) => {
          this.logs = response;
      },
      error: (error) => {
        this.logs = [];
        console.error('Error:', error);
      },
    });
  }

  isFormValid(): boolean {
    if (!this.query && !this.selectedOption.value && !this.startDate && !this.startTime && !this.endDate && !this.endTime) {
      return false;
    }
    return true

    // const startDateTime = new Date(this.startDate);
    // const [startHours, startMinutes] = this.startTime.split(':').map(Number);
    // startDateTime.setHours(startHours, startMinutes);

    // const endDateTime = new Date(this.endDate);
    // const [endHours, endMinutes] = this.endTime.split(':').map(Number);
    // endDateTime.setHours(endHours, endMinutes);

    // return startDateTime <= endDateTime;
  }

  onSubmit() {
    const formatDateTime = (date: Date | null, time: string): string | null => {
      if (!date) return null;
    
      const localDate = new Date(date); 
      const [hours, minutes] = time.split(':').map(Number);
    
      localDate.setHours(hours || 0, minutes || 0, 0, 0);
    
      return new Date(localDate.getTime() - localDate.getTimezoneOffset() * 60000).toISOString();
    };    
  
    const requestBody = {
        query: this.query || null,
        level: this.selectedOption.value ?? null,
        startDate: formatDateTime(this.startDate, this.startTime),
        endDate: formatDateTime(this.endDate, this.endTime),
    };
  
    this.logService.searchLogs(requestBody).subscribe({
        next: (response: any[]) => {
          if (response.length === 0) {
            this.logs = []; 
            this.snackBar.open('No logs found for the given criteria.', 'Close', {
              duration: 3000,
              panelClass: ['snackbar-warning'],
            });
          } else {
            this.logs = response; 
            // this.snackBar.open('Logs successfully retrieved.', 'Close', {
            //   duration: 3000,
            //   panelClass: ['snackbar-success'],
            // });
          }
        },
        error: (error) => {
          this.logs = []; 
          this.snackBar.open('No logs found for the given criteria.', 'Close', {
            duration: 3000,
            panelClass: ['snackbar-error'],
          });
          console.error('Error:', error);
        },
      });
    }

    getLogRowClass(row: any): string {
        switch (row.level) {
          case 'ERR':
            return 'log-row-error'; 
          case 'WRN':
            return 'log-row-warning'; 
          case 'INF':
            return ''; 
          default:
            return 'log-row-info';
        }
    }    
}
