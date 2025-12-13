import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DoutorLayoutComponent } from './layout/doutor-layout.component';
import { PacientesListComponent } from './pacientes/list.component';
import { TratamentosListComponent } from './tratamentos/list.component';

const routes: Routes = [
  {
    path: '',
    component: DoutorLayoutComponent,
    children: [
      { path: '', redirectTo: 'pacientes', pathMatch: 'full' },
      { path: 'pacientes', component: PacientesListComponent },
      { path: 'tratamentos', component: TratamentosListComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DoutorRoutingModule { }

