import { Component, OnInit } from '@angular/core';
import { EspecialidadeService } from '../../../core/services/especialidade.service';
import { Especialidade } from '../../../core/models/especialidade.model';

@Component({
  selector: 'app-especialidades-list',
  templateUrl: './list.component.html',
  styles: []
})
export class EspecialidadesListComponent implements OnInit {
  especialidades: Especialidade[] = [];

  constructor(private especialidadeService: EspecialidadeService) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.especialidadeService.getAll().subscribe({
      next: (response) => {
        this.especialidades = response;
      },
      error: (error) => console.error('Erro ao buscar especialidades:', error)
    });
  }
}

