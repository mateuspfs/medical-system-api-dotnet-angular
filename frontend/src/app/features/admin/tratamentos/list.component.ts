import { Component, OnInit } from '@angular/core';
import { TratamentoService } from '../../../core/services/tratamento.service';
import { EspecialidadeService } from '../../../core/services/especialidade.service';
import { Tratamento } from '../../../core/models/tratamento.model';
import { Especialidade } from '../../../core/models/especialidade.model';
import { PaginatedResponse, PaginationParams } from '../../../core/models/pagination.model';

@Component({
  selector: 'app-tratamentos-list',
  templateUrl: './list.component.html',
  styles: []
})
export class TratamentosListComponent implements OnInit {
  tratamentos: Tratamento[] = [];
  especialidades: Especialidade[] = [];
  totalPages = 0;
  currentPage = 1;
  pageSize = 8;
  filterNome = '';
  filterEspecialidade = '';
  
  showAddModal = false;
  showEditModal = false;
  showDeleteModal = false;
  selectedTratamento: Tratamento | null = null;
  
  newTratamento = { nome: '', tempo: 0, especialidadeId: 0 };
  editTratamento = { nome: '', tempo: 0, especialidadeId: 0 };

  constructor(
    private tratamentoService: TratamentoService,
    private especialidadeService: EspecialidadeService
  ) {}

  ngOnInit(): void {
    this.loadEspecialidades();
    this.loadData();
  }

  loadEspecialidades(): void {
    this.especialidadeService.getAll().subscribe({
      next: (response) => {
        this.especialidades = response.$values;
      },
      error: (error) => console.error('Erro ao buscar especialidades:', error)
    });
  }

  loadData(): void {
    const params: PaginationParams = {
      pageNumber: this.currentPage,
      pageSize: this.pageSize
    };

    this.tratamentoService.filter(this.filterNome, this.filterEspecialidade, params).subscribe({
      next: (response: PaginatedResponse<Tratamento>) => {
        this.tratamentos = response.items.$values;
        this.totalPages = response.totalPages;
      },
      error: (error) => console.error('Erro ao buscar tratamentos:', error)
    });
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadData();
  }

  onFilter(): void {
    this.currentPage = 1;
    this.loadData();
  }

  openAddModal(): void {
    this.newTratamento = { nome: '', tempo: 0, especialidadeId: 0 };
    this.showAddModal = true;
  }

  openEditModal(tratamento: Tratamento): void {
    this.selectedTratamento = tratamento;
    this.editTratamento = {
      nome: tratamento.nome,
      tempo: tratamento.tempo,
      especialidadeId: tratamento.especialidadeId
    };
    this.showEditModal = true;
  }

  openDeleteModal(tratamento: Tratamento): void {
    this.selectedTratamento = tratamento;
    this.showDeleteModal = true;
  }

  addTratamento(): void {
    this.tratamentoService.create(this.newTratamento).subscribe({
      next: () => {
        this.showAddModal = false;
        this.loadData();
      },
      error: (error) => console.error('Erro ao adicionar tratamento:', error)
    });
  }

  updateTratamento(): void {
    if (this.selectedTratamento) {
      this.tratamentoService.update(this.selectedTratamento.id, this.editTratamento).subscribe({
        next: () => {
          this.showEditModal = false;
          this.selectedTratamento = null;
          this.loadData();
        },
        error: (error) => console.error('Erro ao editar tratamento:', error)
      });
    }
  }

  deleteTratamento(): void {
    if (this.selectedTratamento) {
      this.tratamentoService.delete(this.selectedTratamento.id).subscribe({
        next: () => {
          this.showDeleteModal = false;
          this.selectedTratamento = null;
          this.loadData();
        },
        error: (error) => console.error('Erro ao excluir tratamento:', error)
      });
    }
  }
}

