import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'app/env/environment';
import { DockerRepositoryDTO } from 'app/models/models';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RepositoryService {

  constructor(private http: HttpClient) { }
  
  getDockerRepositoryById(id: string) : Observable<DockerRepositoryDTO> {
    return this.http.get<DockerRepositoryDTO>(`${environment.apiHost}dockerRepositories/${id}`);
  }
}
