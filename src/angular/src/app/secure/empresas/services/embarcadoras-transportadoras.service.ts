import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { ApiResponse } from 'src/app/_shared/models/api-response.model';
import { EmbarcadoraTransportadora } from '../models/embarcadoras-transportadoras.model';

@Injectable({
    providedIn: 'root'
})
export class EmbarcadorasTransportadorasService {

    constructor(
        private http: HttpClient,
        @Inject('API_URL') public apiUrl: string
    ){}

    consultarPorId(embarcadoraTransportadoraId: string): Observable<EmbarcadoraTransportadora>{
        return this.http.get<ApiResponse>(`${this.apiUrl}/v1/embarcadoras-transportadoras/${embarcadoraTransportadoraId}`).pipe(
            map((response: any) => JSON.parse(JSON.stringify(response.Data)))
        );
    }

    inserir(embarcadoraTransportadora: EmbarcadoraTransportadora): Observable<any> {
        return this.http.post<ApiResponse>(`${this.apiUrl}/v1/embarcadoras-transportadoras`, JSON.stringify(embarcadoraTransportadora), { headers: new HttpHeaders().set('Content-Type', 'application/json')});
    }

    atualizar(embarcadoraTransportadora: EmbarcadoraTransportadora): Observable<any> {
        return this.http.put<ApiResponse>(`${this.apiUrl}/v1/embarcadoras-transportadoras/${embarcadoraTransportadora.EmbarcadoraTransportadoraId}`, JSON.stringify(embarcadoraTransportadora), { headers: new HttpHeaders().set('Content-Type', 'application/json')});
    }

    apagar(embarcadoraId: number, transportadoraId): Observable<any> {
        return this.http.delete<ApiResponse>(`${this.apiUrl}/v1/embarcadoras-transportadoras?embarcadoraId=${embarcadoraId}&transportadoraId=${transportadoraId}`);
    }
}
