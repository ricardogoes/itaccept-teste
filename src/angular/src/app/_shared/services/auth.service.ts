import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Usuario } from 'src/app/secure/empresas/models/usuarios.model';
import { ApiResponse } from '../models/api-response.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
    constructor(private _http: HttpClient, @Inject('API_URL') private _apiUrl: string) {}

    login(username: string, password: string): Observable<ApiResponse> {
        return this._http
            .post<ApiResponse>(`${this._apiUrl}/v1/auth`, {
                Username: username,
                Password: password
            })
            .pipe(
                map(response => {
                    if (response.State == 1) this.loadSessions(response.Data);
                    return response;
                }),
                catchError(err => throwError(err.error.Message))
            );
    }

    logout() {
        sessionStorage.clear();
    }

    checkLogin(route: string): boolean {
        console.log(route);
        if(!this.getLocalToken())
            return false;

        const usuarioLogado = this.getLoggedUserInfo();
        if(route.includes('empresas') && usuarioLogado.TipoUsuario !== 'Admin')
            return false;
        else if (route.includes('produtos') && (usuarioLogado.TipoUsuario === 'Admin' || usuarioLogado.TipoEmpresa === 'Transportadora'))
            return false;

        return true;
    }

    private loadSessions(json: any) {
        sessionStorage.setItem('token', json.AccessToken);
        sessionStorage.setItem('usuarioLogado', JSON.stringify(json.UsuarioLogado));
    }

    getLoggedUserInfo(): Usuario {
        return JSON.parse(sessionStorage.getItem('usuarioLogado') || '{}');
    }

    getLocalToken(): string {
        return sessionStorage.getItem('token') || '';
    }
}
