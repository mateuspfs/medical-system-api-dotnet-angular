import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { Especialidade } from '../models/especialidade.model';

@Injectable({
  providedIn: 'root'
})
export class EspecialidadeService {
  constructor(private api: ApiService) {}

  getAll(): Observable<Especialidade[]> {
    return this.api.get<Especialidade[]>('/Especialidade');
  }
}

