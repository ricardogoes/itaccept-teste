import { NgModule } from '@angular/core';
import { CpfCnpjPipe } from './cpf-cnpj.pipe';
import { ReplaceLineBreaksPipe } from './replace-line-break.pipe';
import { SafePipe } from './safe.pipe';
import { ShortUrlPipe } from './short-url.pipe';
import { SimNaoPipe } from './sim-nao.pipe';

import { StatusPipe } from './status.pipe';


@NgModule({
  declarations: [
    CpfCnpjPipe,
    ReplaceLineBreaksPipe,
    SafePipe,
    ShortUrlPipe,
    SimNaoPipe,
    StatusPipe
  ],
  exports: [
    CpfCnpjPipe,
    ReplaceLineBreaksPipe,
    SafePipe,
    ShortUrlPipe,
    SimNaoPipe,
    StatusPipe
  ]
})
export class CustomPipesModule { }
