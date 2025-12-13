import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { Tratamento } from '../models/tratamento.model';
import { PaginatedResponse, PaginationParams } from '../models/pagination.model';

@Injectable({
  providedIn: 'root'
})
export class TratamentoService {
  constructor(private api: ApiService) {}

  filter(filterNome: string, filterEspecialidade: string, params: PaginationParams): Observable<PaginatedResponse<Tratamento>> {
    let url = `/Tratamento/filter?pageNumber=${params.pageNumber}&pageSize=${params.pageSize}`;
    if (filterNome) {
      url += `&filterNome=${filterNome}`;
    }
    if (filterEspecialidade) {
      url += `&filterEspecialidade=${filterEspecialidade}`;
    }
    return this.api.get<PaginatedResponse<Tratamento>>(url);
  }

  create(tratamento: { nome: string; tempo: number; especialidadeId: number }): Observable<Tratamento> {
    return this.api.post<Tratamento>('/Tratamento', tratamento);
  }

  update(id: number, tratamento: { nome: string; tempo: number; especialidadeId: number }): Observable<Tratamento> {
    return this.api.put<Tratamento>(`/Tratamento/${id}`, tratamento);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`/Tratamento/${id}`);
  }
}

