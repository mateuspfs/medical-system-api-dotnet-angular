import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AdminRoutingModule } from './admin-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { AdminLayoutComponent } from './layout/admin-layout.component';
import { AdminsListComponent } from './admins/list.component';
import { DoutoresListComponent } from './doutores/list.component';
import { PacientesListComponent } from './pacientes/list.component';
import { TratamentosListComponent } from './tratamentos/list.component';
import { EspecialidadesListComponent } from './especialidades/list.component';

@NgModule({
  declarations: [
    AdminLayoutComponent,
    AdminsListComponent,
    DoutoresListComponent,
    PacientesListComponent,
    TratamentosListComponent,
    EspecialidadesListComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    AdminRoutingModule,
    SharedModule
  ]
})
export class AdminModule { }

