import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { ApiResponse } from 'src/app/_shared/models/api-response.model';
import { Lance } from '../models/lances.model';

@Injectable({
    providedIn: 'root'
})
export class LancesService {

    constructor(
        private http: HttpClient,
        @Inject('API_URL') public apiUrl: string
    ){}

    consultarPorId(lanceId: string): Observable<Lance>{
        return this.http.get<ApiResponse>(`${this.apiUrl}/v1/lances/${lanceId}`).pipe(
            map((response: any) => JSON.parse(JSON.stringify(response.Data)))
        );
    }

    consultarPelaTransportadora(transportadoraId: number, status: string): Observable<Lance[]>{
        return this.http.get<ApiResponse>(`${this.apiUrl}/v1/transportadoras/${transportadoraId}/lances?status=${status}`).pipe(
            map((response: any) => JSON.parse(JSON.stringify(response.Data)))
        );
    }

    consultarPelaOferta(ofertaId: number, status: string): Observable<Lance[]>{
        return this.http.get<ApiResponse>(`${this.apiUrl}/v1/ofertas/${ofertaId}/lances?status=${status}`).pipe(
            map((response: any) => JSON.parse(JSON.stringify(response.Data)))
        );
    }

    inserir(lance: any): Observable<any> {
        return this.http.post<ApiResponse>(`${this.apiUrl}/v1/lances`, JSON.stringify(lance), { headers: new HttpHeaders().set('Content-Type', 'application/json')});
    }

    atualizar(lance: Lance): Observable<any> {
        return this.http.put<ApiResponse>(`${this.apiUrl}/v1/lances/${lance.LanceId}`, JSON.stringify(lance), { headers: new HttpHeaders().set('Content-Type', 'application/json')});
    }

    atualizarStatus(lanceId: number): Observable<any> {
        return this.http.put<ApiResponse>(`${this.apiUrl}/v1/lances/${lanceId}/status`, '');
    }

    atualizarLanceVencedor(lanceId: number): Observable<any> {
        return this.http.put<ApiResponse>(`${this.apiUrl}/v1/lances/${lanceId}/vencedor`, '');
    }
}
