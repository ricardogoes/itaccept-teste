import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SecureComponent } from './secure.component';

const routes: Routes = [
    {
        path: '',
        component: SecureComponent,
        children: [
            {
                path: 'empresas',
                loadChildren: () => import('./empresas/empresas.module').then((m) => m.EmpresasModule)
            },
            {
                path: 'produtos',
                loadChildren: () => import('./produtos/produtos.module').then((m) => m.ProdutosModule)
            },
            {
                path: 'ofertas',
                loadChildren: () => import('./ofertas/ofertas.module').then((m) => m.OfertasModule)
            },
            {
                path: '**',
                loadChildren: () => import('src/app/_shared/components/not-found/not-found.module').then((m) => m.NotFoundModule)
            },
            { path: '', redirectTo: 'ofertas', pathMatch: 'prefix' }
        ]
    }
];

@NgModule({
    imports: [
		  RouterModule.forChild(routes)],
    exports: [
        RouterModule
    ]
})
export class SecureRoutingModule {}
