import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EmpresasDetailComponent } from './components/empresas-detail/empresas-detail.component';
import { EmpresasListComponent } from './components/empresas-list/empresas-list.component';
import { EmpresasRoutingModule } from './empresas-routing.module';
import { MaterialModule } from 'src/app/_shared/components/material/material.module';
import { CustomPipesModule } from 'src/app/_shared/custom-pipes/custom-pipes.module';
import { UsuariosDetailComponent } from './components/usuarios-detail/usuarios-detail.component';
import { UsuariosListComponent } from './components/usuarios-list/usuarios-list.component';
import { AssociarEmpresasComponent } from './components/associar-empresas/associar-empresas.component';

@NgModule({
	declarations: [
        AssociarEmpresasComponent,
		EmpresasDetailComponent,
        EmpresasListComponent,
        UsuariosDetailComponent,
        UsuariosListComponent
    ],
	imports: [
		CommonModule,
		FormsModule,
		ReactiveFormsModule,
        MaterialModule,
        CustomPipesModule,
		EmpresasRoutingModule,
	],
    entryComponents: [
        EmpresasDetailComponent
    ]
})
export class EmpresasModule { }
