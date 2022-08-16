import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SecureRoutingModule } from './secure-routing.module';
import { SecureComponent } from './secure.component';
import { HeaderModule } from './header/header.module';
import { LoadingModule } from '../_shared/components/loading/loading.module';

@NgModule({
    declarations: [
        SecureComponent
    ],
    imports: [
        CommonModule,
        FormsModule, ReactiveFormsModule,
        LoadingModule,
        SecureRoutingModule,
        HeaderModule
    ]
})
export class SecureModule {}
