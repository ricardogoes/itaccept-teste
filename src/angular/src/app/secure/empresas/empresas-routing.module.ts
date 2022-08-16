import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { EmpresasListComponent } from './components/empresas-list/empresas-list.component';

const routes: Routes = [
    {
        path: '',
        children: [
          {
              path: '',
              component: EmpresasListComponent
          }
        ]

    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class EmpresasRoutingModule { }
