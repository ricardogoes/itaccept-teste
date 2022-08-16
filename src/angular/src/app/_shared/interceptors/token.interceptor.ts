import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, throwError } from 'rxjs';
import { LoadingService } from '../components/loading/loading.service';
import { ApiErrorHandler } from '../services/api-error-handler.service';
import { AuthService } from '../services/auth.service';

@Injectable({ providedIn: 'root' })
export class TokenInterceptor implements HttpInterceptor {
    constructor(
        public auth: AuthService,
        private apiErrorHandler: ApiErrorHandler,
        private loadingService: LoadingService
    ) {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        this.loadingService.setLoading(true, request.url);

        request = request.clone({
            setHeaders: {
                Authorization : `Bearer ${this.auth.getLocalToken()}`
            }
        });
        return next.handle(request).pipe(
            map((evt: any) => {
                if (evt instanceof HttpResponse) {
                    this.loadingService.setLoading(false, request.url);
                }

                return evt;
            }),
            catchError((error: HttpErrorResponse) => {
                this.loadingService.setLoading(false, request.url);
                return throwError(() => this.apiErrorHandler.handleError(error));
            })
        );
    }
}
