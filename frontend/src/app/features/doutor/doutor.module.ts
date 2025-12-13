import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DoutorRoutingModule } from './doutor-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { DoutorLayoutComponent } from './layout/doutor-layout.component';
import { PacientesListComponent } from './pacientes/list.component';
import { TratamentosListComponent } from './tratamentos/list.component';

@NgModule({
  declarations: [
    DoutorLayoutComponent,
    PacientesListComponent,
    TratamentosListComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    DoutorRoutingModule,
    SharedModule
  ]
})
export class DoutorModule { }

