import { Component, OnInit } from '@angular/core';
import { DoutorService } from '../../../core/services/doutor.service';
import { EspecialidadeService } from '../../../core/services/especialidade.service';
import { Doutor } from '../../../core/models/doutor.model';
import { Especialidade } from '../../../core/models/especialidade.model';
import { PaginatedResponse, PaginationParams } from '../../../core/models/pagination.model';
import { MaskUtil } from '../../../core/utils/mask.util';

@Component({
  selector: 'app-doutores-list',
  templateUrl: './list.component.html',
  styles: []
})
export class DoutoresListComponent implements OnInit {
  doutores: Doutor[] = [];
  especialidades: Especialidade[] = [];
  totalPages = 0;
  currentPage = 1;
  pageSize = 6;
  search = '';
  filterEspecialidade = '';
  
  showAddModal = false;
  showEditModal = false;
  showDeleteModal = false;
  showInfoModal = false;
  selectedDoutor: Doutor | null = null;
  doutorInfo: Doutor | null = null;
  
  newDoutor = { nome: '', email: '', telefone: '', cpf: '', endereco: '', documento: null as File | null };
  editDoutor = { nome: '', email: '', telefone: '', cpf: '', endereco: '', documento: null as File | null };
  selectedEspecialidades: number[] = [];
  editSelectedEspecialidades: number[] = [];

  constructor(
    private doutorService: DoutorService,
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

    this.doutorService.filter(this.search, this.filterEspecialidade, params).subscribe({
      next: (response: PaginatedResponse<Doutor>) => {
        this.doutores = response.items.$values;
        this.totalPages = response.totalPages;
      },
      error: (error) => console.error('Erro ao buscar doutores:', error)
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
    this.newDoutor = { nome: '', email: '', telefone: '', cpf: '', endereco: '', documento: null };
    this.selectedEspecialidades = [];
    this.showAddModal = true;
  }

  openEditModal(doutor: Doutor): void {
    this.selectedDoutor = doutor;
    this.editDoutor = {
      nome: doutor.nome,
      email: doutor.email,
      telefone: doutor.telefone,
      cpf: doutor.cpf,
      endereco: doutor.endereco,
      documento: null
    };
    const especialidadesArray = doutor.especialidades.split(', ').map(item => item.trim());
    this.editSelectedEspecialidades = this.especialidades
      .filter(esp => especialidadesArray.includes(esp.nome))
      .map(esp => esp.id);
    this.showEditModal = true;
  }

  openDeleteModal(doutor: Doutor): void {
    this.selectedDoutor = doutor;
    this.showDeleteModal = true;
  }

  openInfoModal(doutor: Doutor): void {
    this.doutorService.getById(doutor.id).subscribe({
      next: (doutorInfo) => {
        this.doutorInfo = doutorInfo;
        this.showInfoModal = true;
      },
      error: (error) => console.error('Erro ao buscar informações do doutor:', error)
    });
  }

  onFileSelected(event: any, type: 'add' | 'edit'): void {
    const file = event.target.files[0];
    if (type === 'add') {
      this.newDoutor.documento = file;
    } else {
      this.editDoutor.documento = file;
    }
  }

  toggleEspecialidade(id: number, type: 'add' | 'edit'): void {
    if (type === 'add') {
      const index = this.selectedEspecialidades.indexOf(id);
      if (index > -1) {
        this.selectedEspecialidades.splice(index, 1);
      } else {
        this.selectedEspecialidades.push(id);
      }
    } else {
      const index = this.editSelectedEspecialidades.indexOf(id);
      if (index > -1) {
        this.editSelectedEspecialidades.splice(index, 1);
      } else {
        this.editSelectedEspecialidades.push(id);
      }
    }
  }

  isEspecialidadeSelected(id: number, type: 'add' | 'edit'): boolean {
    if (type === 'add') {
      return this.selectedEspecialidades.includes(id);
    } else {
      return this.editSelectedEspecialidades.includes(id);
    }
  }

  addDoutor(): void {
    const formData = new FormData();
    formData.append('Nome', this.newDoutor.nome);
    formData.append('Email', this.newDoutor.email);
    formData.append('Telefone', MaskUtil.unmask(this.newDoutor.telefone));
    formData.append('CPF', MaskUtil.unmask(this.newDoutor.cpf));
    formData.append('Endereco', this.newDoutor.endereco);
    if (this.newDoutor.documento) {
      formData.append('Documento', this.newDoutor.documento);
    }

    const especialidadeIds = this.selectedEspecialidades.join(', ');
    this.doutorService.create(formData, especialidadeIds).subscribe({
      next: () => {
        this.showAddModal = false;
        this.loadData();
      },
      error: (error) => console.error('Erro ao adicionar doutor:', error)
    });
  }

  updateDoutor(): void {
    if (this.selectedDoutor) {
      const formData = new FormData();
      formData.append('nome', this.editDoutor.nome);
      formData.append('email', this.editDoutor.email);
      formData.append('telefone', MaskUtil.unmask(this.editDoutor.telefone));
      formData.append('cpf', MaskUtil.unmask(this.editDoutor.cpf));
      formData.append('endereco', this.editDoutor.endereco);
      if (this.editDoutor.documento) {
        formData.append('documento', this.editDoutor.documento);
      }

      const especialidadeIds = this.editSelectedEspecialidades.join(', ');
      this.doutorService.update(this.selectedDoutor.id, formData, especialidadeIds).subscribe({
        next: () => {
          this.showEditModal = false;
          this.selectedDoutor = null;
          this.loadData();
        },
        error: (error) => console.error('Erro ao editar doutor:', error)
      });
    }
  }

  deleteDoutor(): void {
    if (this.selectedDoutor) {
      this.doutorService.delete(this.selectedDoutor.id).subscribe({
        next: () => {
          this.showDeleteModal = false;
          this.selectedDoutor = null;
          this.loadData();
        },
        error: (error) => console.error('Erro ao excluir doutor:', error)
      });
    }
  }

  maskPhone(phone: string): string {
    return MaskUtil.maskPhone(phone);
  }

  maskCPF(cpf: string): string {
    return MaskUtil.maskCPF(cpf);
  }

  getDocumentoImage(documentoNome: string | undefined): string {
    if (!documentoNome) return '';
    return this.doutorService.getDocumentoImage(documentoNome);
  }

  onPhoneInput(event: any, field: 'newDoutor' | 'editDoutor'): void {
    const value = event.target.value;
    const masked = MaskUtil.maskPhone(value);
    if (field === 'newDoutor') {
      this.newDoutor.telefone = masked;
    } else {
      this.editDoutor.telefone = masked;
    }
  }

  onCPFInput(event: any, field: 'newDoutor' | 'editDoutor'): void {
    const value = event.target.value;
    const masked = MaskUtil.maskCPF(value);
    if (field === 'newDoutor') {
      this.newDoutor.cpf = masked;
    } else {
      this.editDoutor.cpf = masked;
    }
  }
}

