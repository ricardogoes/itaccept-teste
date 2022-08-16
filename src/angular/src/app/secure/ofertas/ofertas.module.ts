import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { OfertasDetailComponent } from './components/ofertas-detail/ofertas-detail.component';
import { OfertasListComponent } from './components/ofertas-list/ofertas-list.component';
import { OfertasRoutingModule } from './ofertas-routing.module';
import { MaterialModule } from 'src/app/_shared/components/material/material.module';
import { CustomPipesModule } from 'src/app/_shared/custom-pipes/custom-pipes.module';
import { LancesDetailComponent } from './components/lances-detail/lances-detail.component';
import { LancesListComponent } from './components/lances-list/lances-list.component';
import { NgxMaskModule } from 'ngx-mask';
import { OfertasListEmbarcadoraComponent } from './components/ofertas-list-embarcadora/ofertas-list-embarcadora.component';
import { OfertasListTransportadoraComponent } from './components/ofertas-list-transportadora/ofertas-list-transportadora.component';

@NgModule({
  declarations: [
    OfertasDetailComponent,
    OfertasListComponent,
    OfertasListEmbarcadoraComponent,
    OfertasListTransportadoraComponent,
    LancesDetailComponent,
    LancesListComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    CustomPipesModule,
    NgxMaskModule.forRoot(),
    OfertasRoutingModule,
  ],
  exports: [
    OfertasDetailComponent
  ]
})
export class OfertasModule { }
