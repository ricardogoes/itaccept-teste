import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProdutosDetailComponent } from './components/produtos-detail/produtos-detail.component';
import { ProdutosListComponent } from './components/produtos-list/produtos-list.component';
import { ProdutosRoutingModule } from './produtos-routing.module';
import { MaterialModule } from 'src/app/_shared/components/material/material.module';
import { CustomPipesModule } from 'src/app/_shared/custom-pipes/custom-pipes.module';

@NgModule({
	declarations: [
		ProdutosDetailComponent,
        ProdutosListComponent
    ],
	imports: [
		CommonModule,
		FormsModule,
		ReactiveFormsModule,
        MaterialModule,
        CustomPipesModule,
		ProdutosRoutingModule,
	]
})
export class ProdutosModule { }
