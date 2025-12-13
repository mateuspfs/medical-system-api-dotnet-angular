import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminLayoutComponent } from './layout/admin-layout.component';
import { AdminsListComponent } from './admins/list.component';
import { DoutoresListComponent } from './doutores/list.component';
import { PacientesListComponent } from './pacientes/list.component';
import { TratamentosListComponent } from './tratamentos/list.component';
import { EspecialidadesListComponent } from './especialidades/list.component';

const routes: Routes = [
  {
    path: '',
    component: AdminLayoutComponent,
    children: [
      { path: '', redirectTo: 'admins', pathMatch: 'full' },
      { path: 'admins', component: AdminsListComponent },
      { path: 'doutores', component: DoutoresListComponent },
      { path: 'pacientes', component: PacientesListComponent },
      { path: 'tratamentos', component: TratamentosListComponent },
      { path: 'especialidades', component: EspecialidadesListComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }

