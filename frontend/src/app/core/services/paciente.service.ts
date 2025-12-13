import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { Paciente } from '../models/paciente.model';
import { PaginatedResponse, PaginationParams } from '../models/pagination.model';

@Injectable({
  providedIn: 'root'
})
export class PacienteService {
  constructor(private api: ApiService) {}

  filter(search: string, params: PaginationParams): Observable<PaginatedResponse<Paciente>> {
    let url = `/Paciente/filter?pageNumber=${params.pageNumber}&pageSize=${params.pageSize}`;
    if (search) {
      url += `&search=${search}`;
    }
    return this.api.get<PaginatedResponse<Paciente>>(url);
  }

  create(paciente: Partial<Paciente>): Observable<Paciente> {
    return this.api.post<Paciente>('/Paciente', paciente);
  }

  update(id: number, paciente: Partial<Paciente>): Observable<Paciente> {
    return this.api.put<Paciente>(`/Paciente/${id}`, paciente);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`/Paciente/${id}`);
  }
}

