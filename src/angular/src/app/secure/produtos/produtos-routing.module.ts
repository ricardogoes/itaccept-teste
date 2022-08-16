import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ProdutosListComponent } from './components/produtos-list/produtos-list.component';

const routes: Routes = [
    {
        path: '',
        children: [
          {
              path: '',
              component: ProdutosListComponent
          }
        ]

    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ProdutosRoutingModule { }
