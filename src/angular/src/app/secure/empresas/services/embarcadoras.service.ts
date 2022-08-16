import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { ApiResponse } from 'src/app/_shared/models/api-response.model';
import { Empresa } from '../models/empresas.model';

@Injectable({
    providedIn: 'root'
})
export class EmbarcadorasService {

    constructor(
        private http: HttpClient,
        @Inject('API_URL') public apiUrl: string
    ){}

    consultarPorId(embarcadoraId: string): Observable<Empresa>{
        return this.http.get<ApiResponse>(`${this.apiUrl}/v1/embarcadoras/${embarcadoraId}`).pipe(
            map((response: any) => JSON.parse(JSON.stringify(response.Data)))
        );
    }


    consultarPorTransportadora(transportadoraId: string, tipo: string): Observable<Empresa[]>{
        return this.http.get<ApiResponse>(`${this.apiUrl}/v1/transportadoras/${transportadoraId}/embarcadoras?tipo=${tipo}`).pipe(
            map((response: any) => JSON.parse(JSON.stringify(response.Data)))
        );
    }

    consultarEmbarcadoras(status: string): Observable<Empresa[]>{
        return this.http.get<ApiResponse>(`${this.apiUrl}/v1/embarcadoras?status=${status}`).pipe(
            map((response: any) => JSON.parse(JSON.stringify(response.Data)))
        );
    }

    inserir(empresa: any): Observable<any> {
        return this.http.post<ApiResponse>(`${this.apiUrl}/v1/embarcadoras`, JSON.stringify(empresa), { headers: new HttpHeaders().set('Content-Type', 'application/json')});
    }

    atualizar(empresa: Empresa): Observable<any> {
        return this.http.put<ApiResponse>(`${this.apiUrl}/v1/embarcadoras/${empresa.EmpresaId}`, JSON.stringify(empresa), { headers: new HttpHeaders().set('Content-Type', 'application/json')});
    }

    atualizarStatus(empresaId: number): Observable<any> {
        return this.http.put<ApiResponse>(`${this.apiUrl}/v1/embarcadoras/${empresaId}/status`, '');
    }
}
