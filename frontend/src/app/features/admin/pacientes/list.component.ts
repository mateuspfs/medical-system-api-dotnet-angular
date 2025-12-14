import { Component, OnInit } from '@angular/core';
import { PacienteService } from '../../../core/services/paciente.service';
import { Paciente } from '../../../core/models/paciente.model';
import { PaginatedResponse, PaginationParams } from '../../../core/models/pagination.model';
import { MaskUtil } from '../../../core/utils/mask.util';

@Component({
  selector: 'app-pacientes-list',
  templateUrl: './list.component.html',
  styles: []
})
export class PacientesListComponent implements OnInit {
  pacientes: Paciente[] = [];
  totalPages = 0;
  currentPage = 1;
  pageSize = 8;
  search = '';
  
  showAddModal = false;
  showEditModal = false;
  showDeleteModal = false;
  selectedPaciente: Paciente | null = null;
  
  newPaciente = { nome: '', email: '', telefone: '', cpf: '', endereco: '' };
  editPaciente = { codigo: '', nome: '', email: '', telefone: '', cpf: '', endereco: '' };

  constructor(private pacienteService: PacienteService) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    const params: PaginationParams = {
      pageNumber: this.currentPage,
      pageSize: this.pageSize
    };

    this.pacienteService.filter(this.search, params).subscribe({
      next: (response: PaginatedResponse<Paciente>) => {
        this.pacientes = response.items;
        this.totalPages = response.totalPages;
      },
      error: (error) => console.error('Erro ao buscar pacientes:', error)
    });
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadData();
  }

  onSearch(): void {
    this.currentPage = 1;
    this.loadData();
  }

  openAddModal(): void {
    this.newPaciente = { nome: '', email: '', telefone: '', cpf: '', endereco: '' };
    this.showAddModal = true;
  }

  openEditModal(paciente: Paciente): void {
    this.selectedPaciente = paciente;
    this.editPaciente = {
      codigo: paciente.codigo,
      nome: paciente.nome,
      email: paciente.email,
      telefone: paciente.telefone,
      cpf: paciente.cpf,
      endereco: paciente.endereco
    };
    this.showEditModal = true;
  }

  openDeleteModal(paciente: Paciente): void {
    this.selectedPaciente = paciente;
    this.showDeleteModal = true;
  }

  addPaciente(): void {
    const paciente = {
      codigo: '',
      nome: this.newPaciente.nome,
      email: this.newPaciente.email,
      telefone: MaskUtil.unmask(this.newPaciente.telefone),
      cpf: MaskUtil.unmask(this.newPaciente.cpf),
      endereco: this.newPaciente.endereco
    };
    
    this.pacienteService.create(paciente).subscribe({
      next: () => {
        this.showAddModal = false;
        this.loadData();
      },
      error: (error) => console.error('Erro ao adicionar paciente:', error)
    });
  }

  updatePaciente(): void {
    if (this.selectedPaciente) {
      const paciente = {
        codigo: this.editPaciente.codigo,
        nome: this.editPaciente.nome,
        email: this.editPaciente.email,
        telefone: MaskUtil.unmask(this.editPaciente.telefone),
        cpf: MaskUtil.unmask(this.editPaciente.cpf),
        endereco: this.editPaciente.endereco
      };
      
      this.pacienteService.update(this.selectedPaciente.id, paciente).subscribe({
        next: () => {
          this.showEditModal = false;
          this.selectedPaciente = null;
          this.loadData();
        },
        error: (error) => console.error('Erro ao editar paciente:', error)
      });
    }
  }

  deletePaciente(): void {
    if (this.selectedPaciente) {
      this.pacienteService.delete(this.selectedPaciente.id).subscribe({
        next: () => {
          this.showDeleteModal = false;
          this.selectedPaciente = null;
          this.loadData();
        },
        error: (error) => console.error('Erro ao excluir paciente:', error)
      });
    }
  }

  maskPhone(phone: string): string {
    return MaskUtil.maskPhone(phone);
  }

  maskCPF(cpf: string): string {
    return MaskUtil.maskCPF(cpf);
  }

  onPhoneInput(event: any, field: 'newPaciente' | 'editPaciente'): void {
    const value = event.target.value;
    const masked = MaskUtil.maskPhone(value);
    if (field === 'newPaciente') {
      this.newPaciente.telefone = masked;
    } else {
      this.editPaciente.telefone = masked;
    }
  }

  onCPFInput(event: any, field: 'newPaciente' | 'editPaciente'): void {
    const value = event.target.value;
    const masked = MaskUtil.maskCPF(value);
    if (field === 'newPaciente') {
      this.newPaciente.cpf = masked;
    } else {
      this.editPaciente.cpf = masked;
    }
  }
}

