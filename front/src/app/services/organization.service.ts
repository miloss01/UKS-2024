import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from "../env/environment";

@Injectable({
  providedIn: 'root'
})
export class OrganizationService {

  private apiUrl = `${environment.apiHost}organization`;

  constructor(private http: HttpClient) { }

  addOrganization(organization: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, organization);
  }

  getOrganizations(email: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/${email}`);
  }

  getMembersByOrganizationId(id: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}/members`);
  }

  addMemberToOrganization(organizationId: string, userId: string): Observable<any> {
    const body = {
      OrganizationId: organizationId,
      UserId: userId,
    };

    return this.http.post(`${this.apiUrl}/add-member`, body);
  }

  // getOrganizationById(id: string): Observable<any> {
  //   return this.http.get<any>(`${this.apiUrl}/details/${id}`);
  // }
}