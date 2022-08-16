import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { ApiResponse } from 'src/app/_shared/models/api-response.model';
import { Empresa } from '../models/empresas.model';

@Injectable({
    providedIn: 'root'
})
export class TransportadorasService {

    constructor(
        private http: HttpClient,
        @Inject('API_URL') public apiUrl: string
    ){}

    consultarPorId(transportadoraId: string): Observable<Empresa>{
        return this.http.get<ApiResponse>(`${this.apiUrl}/v1/transportadoras/${transportadoraId}`).pipe(
            map((response: any) => JSON.parse(JSON.stringify(response.Data)))
        );
    }

    consultarTransportadoras(status: string): Observable<Empresa[]>{
        return this.http.get<ApiResponse>(`${this.apiUrl}/v1/transportadoras?status=${status}`).pipe(
            map((response: any) => JSON.parse(JSON.stringify(response.Data)))
        );
    }

    consultarPorEmbarcadora(embarcadoraId: string, tipo: string): Observable<Empresa[]>{
        return this.http.get<ApiResponse>(`${this.apiUrl}/v1/embarcadoras/${embarcadoraId}/transportadoras?tipo=${tipo}`).pipe(
            map((response: any) => JSON.parse(JSON.stringify(response.Data)))
        );
    }

    inserir(empresa: any): Observable<any> {
        return this.http.post<ApiResponse>(`${this.apiUrl}/v1/transportadoras`, JSON.stringify(empresa), { headers: new HttpHeaders().set('Content-Type', 'application/json')});
    }

    atualizar(empresa: Empresa): Observable<any> {
        return this.http.put<ApiResponse>(`${this.apiUrl}/v1/transportadoras/${empresa.EmpresaId}`, JSON.stringify(empresa), { headers: new HttpHeaders().set('Content-Type', 'application/json')});
    }

    atualizarStatus(empresaId: number): Observable<any> {
        return this.http.put<ApiResponse>(`${this.apiUrl}/v1/transportadoras/${empresaId}/status`, '');
    }
}
