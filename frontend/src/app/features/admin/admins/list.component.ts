import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../../core/services/admin.service';
import { Admin } from '../../../core/models/admin.model';
import { PaginatedResponse, PaginationParams } from '../../../core/models/pagination.model';

@Component({
  selector: 'app-admins-list',
  templateUrl: './list.component.html',
  styles: []
})
export class AdminsListComponent implements OnInit {
  admins: Admin[] = [];
  totalPages = 0;
  currentPage = 1;
  pageSize = 8;
  search = '';
  
  showAddModal = false;
  showEditModal = false;
  showDeleteModal = false;
  selectedAdmin: Admin | null = null;
  
  newAdmin = { name: '', email: '' };
  editAdmin = { name: '', email: '' };

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    const params: PaginationParams = {
      pageNumber: this.currentPage,
      pageSize: this.pageSize
    };

    if (this.search) {
      this.adminService.filter(this.search, params).subscribe({
        next: (response: PaginatedResponse<Admin>) => {
          this.admins = response.items.$values;
          this.totalPages = response.totalPages;
        },
        error: (error) => console.error('Erro ao buscar admins:', error)
      });
    } else {
      this.adminService.getAll(params).subscribe({
        next: (response: PaginatedResponse<Admin>) => {
          this.admins = response.items.$values;
          this.totalPages = response.totalPages;
        },
        error: (error) => console.error('Erro ao buscar admins:', error)
      });
    }
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
    this.newAdmin = { name: '', email: '' };
    this.showAddModal = true;
  }

  openEditModal(admin: Admin): void {
    this.selectedAdmin = admin;
    this.editAdmin = { name: admin.name, email: admin.email };
    this.showEditModal = true;
  }

  openDeleteModal(admin: Admin): void {
    this.selectedAdmin = admin;
    this.showDeleteModal = true;
  }

  addAdmin(): void {
    this.adminService.create(this.newAdmin).subscribe({
      next: () => {
        this.showAddModal = false;
        this.loadData();
      },
      error: (error) => console.error('Erro ao adicionar admin:', error)
    });
  }

  updateAdmin(): void {
    if (this.selectedAdmin) {
      this.adminService.update(this.selectedAdmin.id, this.editAdmin).subscribe({
        next: () => {
          this.showEditModal = false;
          this.selectedAdmin = null;
          this.loadData();
        },
        error: (error) => console.error('Erro ao editar admin:', error)
      });
    }
  }

  deleteAdmin(): void {
    if (this.selectedAdmin) {
      this.adminService.delete(this.selectedAdmin.id).subscribe({
        next: () => {
          this.showDeleteModal = false;
          this.selectedAdmin = null;
          this.loadData();
        },
        error: (error) => console.error('Erro ao excluir admin:', error)
      });
    }
  }
}

