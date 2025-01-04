import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { LogsService } from 'app/services/logs.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-logs',
  standalone: true,
  imports: [MaterialModule, FormsModule, CommonModule, ReactiveFormsModule],
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

  displayedColumns: string[] = ['timestamp', 'level', 'message']; 

  constructor(private logService: LogsService, private snackBar: MatSnackBar) {}

  onSubmit() {
    const formatDateTime = (date: Date | null, time: string): string | null => {
        if (!date) return null;
        const dateStr = date.toISOString().split('T')[0]; 
        return time ? `${dateStr}T${time}:00` : `${dateStr}T00:00:00`; 
    };
  
    const requestBody = {
        query: this.query || null,
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
}
