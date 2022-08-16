import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { OfertasListComponent } from './components/ofertas-list/ofertas-list.component';

const routes: Routes = [
    {
        path: '',
        children: [
          {
              path: '',
              component: OfertasListComponent
          }
        ]

    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class OfertasRoutingModule { }
