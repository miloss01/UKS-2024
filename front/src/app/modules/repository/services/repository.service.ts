import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'app/env/environment';
import RepositoryCreation, { DescriptionRequest, Repository, VisibilityRequest } from 'app/models/models';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RepositoryService {

  constructor(private http: HttpClient) { }

CreateRepository(requestRepo: RepositoryCreation) : Observable<Repository> {
    const url = `${environment.apiHost}dockerRepositories`;
    return this.http.post<Repository>(url, requestRepo);    
  }

UpdateRepositoryDescription(descritionRequest: DescriptionRequest) : Observable<Repository> {
    const url = `${environment.apiHost}dockerRepositories/update-description`;
    return this.http.put<Repository>(url, descritionRequest);    
  }

UpdateRepositoryVisibility(visibilityRequest: VisibilityRequest) : Observable<Repository> {
  const url = `${environment.apiHost}dockerRepositories/update-visibility`;
  return this.http.put<Repository>(url, visibilityRequest);    
}

DeleteRepository(id: string) : Observable<Repository> {
  const url = `${environment.apiHost}dockerRepositories/delete/${id}`;
  return this.http.delete<Repository>(url);    
}

// GetUsersRepositories(userId: string) : Observable<Repository> {
//   const url = `${environment.apiHost}dockerRepositories/`;
//   return this.http.post<Repository>(url, visibilityRequest);    
// }
}

