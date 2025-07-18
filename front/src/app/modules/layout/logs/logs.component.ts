import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'app/infrastructure/material/material.module';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { LogsService } from 'app/services/logs.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSelectModule } from '@angular/material/select';
import { Chart, registerables } from 'chart.js';
import { ILogs } from 'app/models/models';

Chart.register(...registerables);

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
  logs: ILogs[] = [];
  options = [
    { label: 'Select level', value: '' },
    { label: 'Info', value: 'inf' },
    { label: 'Warning', value: 'wrn' },
    { label: 'Error', value: 'err' },
  ];
  selectedOption: { label: string, value: string } = this.options[0];

  displayedColumns: string[] = ['timestamp', 'level', 'message'];
  
  chart: Chart | null = null;
  
  constructor(private logService: LogsService, private snackBar: MatSnackBar) {}

  ngOnInit(): void {
    this.loadInitialLogs();
  }

  loadInitialLogs(): void {
    this.logService.searchLogs({ query: null, level: null, startDate: null, endDate: null }).subscribe({
      next: (response: any[]) => {
          this.logs = response;
          this.renderChart();
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
    return this.checkDateAndTime();
  }

  checkDateAndTime(): boolean {
    let startDateTime = null;
    let endDateTime = null;
    if(this.startDate) {
      startDateTime = new Date(this.startDate);
      const [startHours, startMinutes] = this.startTime.split(':').map(Number);
      startDateTime.setHours(startHours, startMinutes);
    }
    if(this.endDate) {
      endDateTime = new Date(this.endDate);
      const [endHours, endMinutes] = this.endTime.split(':').map(Number);
      endDateTime.setHours(endHours, endMinutes);
    }
    if(startDateTime && endDateTime) return startDateTime <= endDateTime;
    return true
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
        query: this.query.toLowerCase() || null,
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
          this.renderChart();
        },
        error: (error) => {
          this.logs = []; 
          this.renderChart();
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

  clearForm() {
    this.query = '';
    this.selectedOption = this.options[0];
    this.startDate = null;
    this.startTime = '';
    this.endDate = null;
    this.endTime = '';
  }    
  
  onStartDateChange(event: any) {
    if (event.value) {
      this.startTime = '00:00';
    }
  }
  
  onEndDateChange(event: any) {
    if (event.value) {
      this.endTime = '23:59';
    }
  }

  onStartTimeChange(): void {
    if (!this.startDate) {
      this.startDate = new Date();
    }
  }
  
  onEndTimeChange(): void {
    if (!this.endDate) {
      this.endDate = new Date();
    }
  }
  
  renderChart() {
    const groupedLogs: {
      [minuteKey: string]: { inf: number; wrn: number; err: number };
    } = {};
  
    for (const log of this.logs) {
      const date = new Date(log.timestamp);
      
      const minuteKey = date.toISOString().slice(0, 16).replace('T', ' ');
  
      if (!groupedLogs[minuteKey]) {
        groupedLogs[minuteKey] = { inf: 0, wrn: 0, err: 0 };
      }
  
      type LogLevel = 'inf' | 'wrn' | 'err';

      const rawLevel = log.level?.toLowerCase();
      if (rawLevel === 'inf' || rawLevel === 'wrn' || rawLevel === 'err') {
        console.log("usao")
        const level = rawLevel as LogLevel;
        groupedLogs[minuteKey][level]++;
      }
    }
  
    const sortedKeys = Object.keys(groupedLogs).sort();
    const labels = sortedKeys;
    console.log(groupedLogs)
    const infoData = sortedKeys.map(key => groupedLogs[key].inf);
    const warningData = sortedKeys.map(key => groupedLogs[key].wrn);
    const errorData = sortedKeys.map(key => groupedLogs[key].err);
  
    const config = {
      type: 'line',
      data: {
        labels,
        datasets: [
          {
            label: 'Info',
            data: infoData,
            borderColor: 'blue',
            fill: false
          },
          {
            label: 'Warning',
            data: warningData,
            borderColor: 'orange',
            fill: false
          },
          {
            label: 'Error',
            data: errorData,
            borderColor: 'red',
            fill: false
          }
        ]
      },
      options: {
        responsive: true,
        plugins: {
          legend: {
            position: 'top'
          }
        },
        scales: {
          x: {
            title: {
              display: true,
              text: 'Time (per minutes)'
            }
          },
          y: {
            beginAtZero: true,
            title: {
              display: true,
              text: 'Number of logs'
            }
          }
        }
      }
    };
  
    if (this.chart) {
      this.chart.destroy();
    }
  
    const canvas = document.getElementById('logChart') as HTMLCanvasElement;
    if (canvas) {
      this.chart = new Chart(canvas, config as any);
    }
  }
}
