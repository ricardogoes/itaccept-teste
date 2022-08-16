import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanActivateChild, CanLoad, Route, Router, RouterStateSnapshot, UrlSegment, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate, CanActivateChild, CanLoad {

    constructor(
        private router: Router,
        private authService: AuthService
    ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        if (this.authService.checkLogin(state.url)) {
            // TODO: implementar codigo para não acessar diretamente pela url
            return true;
        }

        alert('User not authenticated');
        this.router.navigate(['/not-found']);
        return false;
    }

    canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        return this.canActivate(route, state);
    }

    canLoad(route: Route, segments: UrlSegment[]): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {

        const fullPath = segments.reduce((path, currentSegment) => {
            return `${path}/${currentSegment.path}`;
          }, '');

        if (this.authService.checkLogin(fullPath)) {
            return true;
        }

        this.router.navigate(['/not-found']);
        return false;
    }
}
