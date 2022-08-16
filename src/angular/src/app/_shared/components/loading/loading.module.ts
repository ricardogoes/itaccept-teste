import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { LoadingComponent } from './loading.component';

@NgModule({
    declarations: [
        LoadingComponent
    ],
    imports: [
        CommonModule,
        MatProgressBarModule
    ],
    exports: [
        LoadingComponent
    ],

})
export class LoadingModule {}
