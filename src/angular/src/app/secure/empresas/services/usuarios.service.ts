import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { ApiResponse } from 'src/app/_shared/models/api-response.model';
import { Usuario } from '../models/usuarios.model';

@Injectable({
    providedIn: 'root'
})
export class UsuariosService {

    constructor(
        private http: HttpClient,
        @Inject('API_URL') public apiUrl: string
    ){}

    consultarPorId(usuarioId: string): Observable<Usuario>{
        return this.http.get<ApiResponse>(`${this.apiUrl}/v1/usuarios/${usuarioId}`).pipe(
            map((response: any) => JSON.parse(JSON.stringify(response.Data)))
        );
    }

    consultarPorEmbarcadora(embarcadoraId: number, status: string): Observable<Usuario[]>{
        return this.http.get<ApiResponse>(`${this.apiUrl}/v1/embarcadoras/${embarcadoraId}/usuarios?status=${status}`).pipe(
            map((response: any) => JSON.parse(JSON.stringify(response.Data)))
        );
    }

    consultarPorTransportadora(transportadoraId: number, status: string): Observable<Usuario[]>{
        return this.http.get<ApiResponse>(`${this.apiUrl}/v1/transportadoras/${transportadoraId}/usuarios?status=${status}`).pipe(
            map((response: any) => JSON.parse(JSON.stringify(response.Data)))
        );
    }

    inserir(usuario: any): Observable<any> {
        return this.http.post<ApiResponse>(`${this.apiUrl}/v1/usuarios`, JSON.stringify(usuario), { headers: new HttpHeaders().set('Content-Type', 'application/json')});
    }

    atualizar(usuario: Usuario): Observable<any> {
        return this.http.put<ApiResponse>(`${this.apiUrl}/v1/usuarios/${usuario.UsuarioId}`, JSON.stringify(usuario), { headers: new HttpHeaders().set('Content-Type', 'application/json')});
    }

    atualizarStatus(usuarioId: number): Observable<any> {
        return this.http.put<ApiResponse>(`${this.apiUrl}/v1/usuarios/${usuarioId}/status`, '');
    }
}
