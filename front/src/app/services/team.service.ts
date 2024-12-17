import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'app/env/environment';
import { TeamsData } from 'app/models/models';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TeamService {

  constructor(private http: HttpClient) { }

  getTeams(id: string): Observable<TeamsData[]>{
    return this.http.get<TeamsData[]>(`${environment.apiHost}team/${id}`);
  }
}
