import { Component } from '@angular/core';

@Component({
  selector: 'app-doutor-layout',
  templateUrl: './doutor-layout.component.html',
  styles: []
})
export class DoutorLayoutComponent {
  menuItems = [
    { label: 'Pacientes', route: '/doutor/pacientes' },
    { label: 'Tratamentos', route: '/doutor/tratamentos' }
  ];
}

