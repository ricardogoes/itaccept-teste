import { LocationStrategy, PathLocationStrategy } from '@angular/common';
import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './_shared/services/auth-guard.service';

const routes: Routes = [
    {
        path: '',
        loadChildren: () => import('./login/login.module').then((m) => m.LoginModule)
    },
    {
        path: 'secure',
        loadChildren: () => import('./secure/secure.module').then((m) => m.SecureModule),
        canLoad: [ AuthGuard ], canActivate: [ AuthGuard ]
    },
    {
        path: '**',
        loadChildren: () => import('src/app/_shared/components/not-found/not-found.module').then((m) => m.NotFoundModule)
    }
];

@NgModule({
    imports: [
        RouterModule.forRoot(routes, {
          scrollPositionRestoration: 'enabled',
          onSameUrlNavigation: 'reload',
          preloadingStrategy: PreloadAllModules,
          useHash: false,
          relativeLinkResolution: 'corrected'
        })
    ],
    exports: [
        RouterModule
    ],
    providers: [
        { provide: LocationStrategy, useClass: PathLocationStrategy }
    ]
})
export class AppRoutingModule { }
