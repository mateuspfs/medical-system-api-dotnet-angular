import { Component } from '@angular/core';

@Component({
  selector: 'app-admin-layout',
  templateUrl: './admin-layout.component.html',
  styles: []
})
export class AdminLayoutComponent {
  menuItems = [
    { label: 'Admins', route: '/admin/admins' },
    { label: 'Doutores', route: '/admin/doutores' },
    { label: 'Pacientes', route: '/admin/pacientes' },
    { label: 'Tratamentos', route: '/admin/tratamentos' },
    { label: 'Especialidades', route: '/admin/especialidades' }
  ];
}

