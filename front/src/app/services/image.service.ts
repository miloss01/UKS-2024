import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'app/env/environment';

@Injectable({
  providedIn: 'root'
})
export class ImageService {
  private apiUrl = `${environment.apiHost}images`;

  constructor(private http: HttpClient) {}

  getImageUrl(fileName: string): Observable<any> {
    const body = { fileName }; 
    return this.http.put<{ imageUrl: string }>(`${this.apiUrl}/get-image-url`, body);
  }

  
  uploadImage(filePath: string, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file); 
    formData.append('filePath', filePath);

    const url = `${this.apiUrl}/upload-image`;
    return this.http.post(url, formData, { responseType: 'text' });
  }
}
