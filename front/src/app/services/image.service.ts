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

  // Metod za dobijanje URL-a slike
  getImageUrl(fileName: string): Observable<string> {
    const url = `${this.apiUrl}/get-image-url?fileName=${encodeURIComponent(fileName)}`;
    return this.http.get<string>(url);
  }

  // Metod za upload slike
  uploadImage(filePath: string, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file); 
    formData.append('filePath', filePath);

    const url = `${this.apiUrl}/upload-image`;
    return this.http.post(url, formData, { responseType: 'text' });
  }
}
