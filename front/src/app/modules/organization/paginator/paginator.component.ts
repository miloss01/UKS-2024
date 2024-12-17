import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-paginator',
  standalone: true,
  templateUrl: './paginator.component.html',
  styleUrls: ['./paginator.component.css']
})
export class PaginatorComponent {
  @Input() currentPage: number = 1;  // Trenutna strana
  @Input() totalItems: number = 0;   // Ukupno stavki (filtriranih podataka)
  @Input() pageSize: number = 2;     // Veličina stranice

  @Output() pageChange = new EventEmitter<number>();   // Za promenu stranice
  @Output() pageSizeChange = new EventEmitter<number>(); // Za promenu veličine stranice

  get totalPages(): number {
    console.log(this.totalItems)
    console.log(this.pageSize)
    console.log("==========================")
    return Math.ceil(this.totalItems / this.pageSize);
  }

  onPageChange(offset: number) {
    const newPage = this.currentPage + offset;
    if (newPage >= 1 && newPage <= this.totalPages) {
      this.pageChange.emit(newPage);
    }
  }

  onPageSizeChange(event: Event) {
    const target = event.target as HTMLSelectElement;
    const newPageSize = Number(target.value);
    this.pageSizeChange.emit(newPageSize);
  }
}
