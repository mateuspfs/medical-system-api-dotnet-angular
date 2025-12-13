import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { Doutor } from '../models/doutor.model';
import { PaginatedResponse, PaginationParams } from '../models/pagination.model';

@Injectable({
  providedIn: 'root'
})
export class DoutorService {
  constructor(private api: ApiService) {}

  filter(search: string, filterEspecialidade: string, params: PaginationParams): Observable<PaginatedResponse<Doutor>> {
    let url = `/Doutor/filter?pageNumber=${params.pageNumber}&pageSize=${params.pageSize}`;
    if (search) {
      url += `&search=${search}`;
    }
    if (filterEspecialidade) {
      url += `&filterEspecialidade=${filterEspecialidade}`;
    }
    return this.api.get<PaginatedResponse<Doutor>>(url);
  }

  getById(id: number): Observable<Doutor> {
    return this.api.get<Doutor>(`/Doutor/${id}`);
  }

  getDocumentoImage(documentoNome: string): string {
    return `https://localhost:7225/api/Doutor/imagem/${documentoNome}`;
  }

  create(formData: FormData, especialidadeIds: string): Observable<Doutor> {
    return this.api.post<Doutor>(`/Doutor?especialidadeIds=${especialidadeIds}`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });
  }

  update(id: number, formData: FormData, especialidadeIds: string): Observable<Doutor> {
    return this.api.put<Doutor>(`/Doutor/${id}?especialidadeIds=${especialidadeIds}`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`/Doutor/${id}`);
  }

  getTratamentosDoutor(token: string, filterNome: string, filterEspecialidade: string, params: PaginationParams): Observable<PaginatedResponse<any>> {
    let url = `/Doutor/TratamentosDoutor?token=${token}&pageNumber=${params.pageNumber}&pageSize=${params.pageSize}`;
    if (filterNome) {
      url += `&filterNome=${filterNome}`;
    }
    if (filterEspecialidade) {
      url += `&filterEspecialidade=${filterEspecialidade}`;
    }
    return this.api.get<PaginatedResponse<any>>(url);
  }

  getEspecialidadesDoutor(token: string): Observable<{ $values: any[] }> {
    return this.api.get<{ $values: any[] }>(`/Doutor/EspecialidadesDoutor?token=${token}`);
  }
}

