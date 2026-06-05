import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Citizen {
  name: string;
  cpf: string;
}

@Injectable({
  providedIn: 'root'
})
export class CitizenService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5052/api/Citizen';

  getAll(): Observable<Citizen[]> {
    return this.http.get<Citizen[]>(this.apiUrl);
  }

  getByName(name: string): Observable<Citizen[]> {
    return this.http.get<Citizen[]>(`${this.apiUrl}/name/${encodeURIComponent(name)}`);
  }

  getByCpf(cpf: string): Observable<Citizen> {
    return this.http.get<Citizen>(`${this.apiUrl}/cpf/${encodeURIComponent(cpf)}`);
  }

  create(citizen: Citizen): Observable<Citizen> {
    return this.http.post<Citizen>(this.apiUrl, citizen);
  }
}
