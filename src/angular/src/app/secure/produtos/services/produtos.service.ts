import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { ApiResponse } from 'src/app/_shared/models/api-response.model';
import { Produto } from '../models/produtos.model';

@Injectable({
    providedIn: 'root'
})
export class ProdutosService {

    constructor(
        private http: HttpClient,
        @Inject('API_URL') public apiUrl: string
    ){}

    consultarPorId(produtoId: string): Observable<Produto>{
        return this.http.get<ApiResponse>(`${this.apiUrl}/v1/produtos/${produtoId}`).pipe(
            map((response: any) => JSON.parse(JSON.stringify(response.Data)))
        );
    }

    consultarPorEmbarcadora(embarcadoraId: number, status: string): Observable<Produto[]>{
        return this.http.get<ApiResponse>(`${this.apiUrl}/v1/embarcadoras/${embarcadoraId}/produtos?status=${status}`).pipe(
            map((response: any) => JSON.parse(JSON.stringify(response.Data)))
        );
    }

    inserir(produto: any): Observable<any> {
        return this.http.post<ApiResponse>(`${this.apiUrl}/v1/produtos`, JSON.stringify(produto), { headers: new HttpHeaders().set('Content-Type', 'application/json')});
    }

    atualizar(produto: Produto): Observable<any> {
        return this.http.put<ApiResponse>(`${this.apiUrl}/v1/produtos/${produto.ProdutoId}`, JSON.stringify(produto), { headers: new HttpHeaders().set('Content-Type', 'application/json')});
    }

    atualizarStatus(produtoId: string): Observable<any> {
        return this.http.put<ApiResponse>(`${this.apiUrl}/v1/produtos/${produtoId}/status`, '');
    }
}
