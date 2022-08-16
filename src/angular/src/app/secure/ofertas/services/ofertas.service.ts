import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { ApiResponse } from 'src/app/_shared/models/api-response.model';
import { Oferta } from '../models/ofertas.model';

@Injectable({
    providedIn: 'root'
})
export class OfertasService {

    constructor(
        private http: HttpClient,
        @Inject('API_URL') public apiUrl: string
    ){}

    consultarPorId(ofertaId: string): Observable<Oferta>{
        return this.http.get<ApiResponse>(`${this.apiUrl}/v1/ofertas/${ofertaId}`).pipe(
            map((response: any) => JSON.parse(JSON.stringify(response.Data)))
        );
    }

    consultarPorEmbarcadora(embarcadoraId: number, status: string): Observable<Oferta[]>{
        return this.http.get<ApiResponse>(`${this.apiUrl}/v1/embarcadoras/${embarcadoraId}/ofertas?status=${status}`).pipe(
            map((response: any) => JSON.parse(JSON.stringify(response.Data)))
        );
    }

    consultarPorTransportadora(transportadoraId: number): Observable<Oferta[]>{
        return this.http.get<ApiResponse>(`${this.apiUrl}/v1/transportadoras/${transportadoraId}/ofertas`).pipe(
            map((response: any) => JSON.parse(JSON.stringify(response.Data)))
        );
    }

    consultarPorProduto(produtoId: number, status: string): Observable<Oferta[]>{
        return this.http.get<ApiResponse>(`${this.apiUrl}/v1/produtos/${produtoId}/ofertas?status=${status}`).pipe(
            map((response: any) => JSON.parse(JSON.stringify(response.Data)))
        );
    }

    inserir(oferta: any): Observable<any> {
        return this.http.post<ApiResponse>(`${this.apiUrl}/v1/ofertas`, JSON.stringify(oferta), { headers: new HttpHeaders().set('Content-Type', 'application/json')});
    }

    atualizar(oferta: Oferta): Observable<any> {
        return this.http.put<ApiResponse>(`${this.apiUrl}/v1/ofertas/${oferta.OfertaId}`, JSON.stringify(oferta), { headers: new HttpHeaders().set('Content-Type', 'application/json')});
    }

    atualizarStatus(ofertaId: number): Observable<any> {
        return this.http.put<ApiResponse>(`${this.apiUrl}/v1/ofertas/${ofertaId}/status`, '');
    }
}
