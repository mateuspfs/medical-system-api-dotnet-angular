import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { Admin } from '../models/admin.model';
import { PaginatedResponse, PaginationParams } from '../models/pagination.model';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  constructor(private api: ApiService) {}

  getAll(params: PaginationParams): Observable<PaginatedResponse<Admin>> {
    return this.api.get<PaginatedResponse<Admin>>(
      `/Admin?pageNumber=${params.pageNumber}&pageSize=${params.pageSize}`
    );
  }

  filter(search: string, params: PaginationParams): Observable<PaginatedResponse<Admin>> {
    return this.api.get<PaginatedResponse<Admin>>(
      `/Admin/filter?search=${search}&pageNumber=${params.pageNumber}&pageSize=${params.pageSize}`
    );
  }

  create(admin: { name: string; email: string }): Observable<Admin> {
    return this.api.post<Admin>('/Admin/', admin);
  }

  update(id: number, admin: { name: string; email: string }): Observable<Admin> {
    return this.api.put<Admin>(`/Admin/${id}`, admin);
  }

  delete(id: number): Observable<void> {
    return this.api.delete<void>(`/Admin/${id}`);
  }
}

