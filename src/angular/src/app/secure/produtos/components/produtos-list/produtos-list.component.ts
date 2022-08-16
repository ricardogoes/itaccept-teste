import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Title } from '@angular/platform-browser';
import { Observable, map, catchError, EMPTY } from 'rxjs';
import { Usuario } from 'src/app/secure/empresas/models/usuarios.model';
import { OfertasDetailComponent } from 'src/app/secure/ofertas/components/ofertas-detail/ofertas-detail.component';
import { ApiResponse } from 'src/app/_shared/models/api-response.model';
import { AuthService } from 'src/app/_shared/services/auth.service';
import { Produto } from '../../models/produtos.model';
import { ProdutosService } from '../../services/produtos.service';
import { ProdutosDetailComponent } from '../produtos-detail/produtos-detail.component';

@Component({
    selector: 'itaccept-produtos-list',
    templateUrl: 'produtos-list.component.html'
})
export class ProdutosListComponent implements OnInit {

    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatSort) set matSort(sort: MatSort) {
        this.dataSource.sort = sort;
    }

    usuarioLogado: Usuario;
    dataSource = new MatTableDataSource<Produto>();
    colunas: string[] = ['NomeProduto', 'Status', 'Acoes'];
    defaultFilterPredicate?: (data: any, filter: string) => boolean;

    produtos$: Observable<Produto[]>;

    filtroDataSource: string;
    itemPesquisado: string;

    constructor(
        private titleService: Title,
        private authService: AuthService,
        private produtosService: ProdutosService,
        private snackBar: MatSnackBar,
        private dialog: MatDialog
    ) {
        this.titleService.setTitle('ItAccept | Produtos');
    }

    ngOnInit(): void {
        this.usuarioLogado = this.authService.getLoggedUserInfo();
        this.filtroDataSource = 'todos';
        this.consultarProdutos();
    }

    consultarProdutos(): void {
        this.produtos$ = this.produtosService.consultarPorEmbarcadora(this.usuarioLogado.EmpresaId, 'todos').pipe(
            map(produtos => {
                this.dataSource = new MatTableDataSource<Produto>([...produtos]);
                this.dataSource.paginator = this.paginator;
                this.defaultFilterPredicate = this.dataSource.filterPredicate;

                return produtos;
            }),
            catchError(error => {
                this.snackBar.open(error, '', { duration: 2000 });
                return EMPTY
            })
        );
    }

    aplicarFiltro = (value: string) => {
        this.dataSource.filterPredicate = this.defaultFilterPredicate!;
        this.dataSource.filter = value.trim().toLocaleLowerCase();
    }

    filtrarDataSource(filtro: string): void {
        this.filtroDataSource = filtro;

        switch (filtro) {
            case 'todos': {
                this.dataSource.filterPredicate = this.defaultFilterPredicate!;
                this.dataSource.filter = '';
                break;
            }
            case 'ativos': {
                this.dataSource.filterPredicate = (data: Produto, filter: string) => !filter || data.Status === (filter === 'ativos' ? true : false);
                this.dataSource.filter = filtro;
                break;
            }
            case 'arquivados': {
                this.dataSource.filterPredicate = (data: Produto, filter: string) => !filter || data.Status === (filter === 'arquivados' ? false : true);
                this.dataSource.filter = filtro;
                break;
            }
        }
    }

    abrirDetalhes(produtoId: string): void {
        const dialogConfig = new MatDialogConfig();
        dialogConfig.width = '900px';
        dialogConfig.disableClose = true;
        dialogConfig.data = { ProdutoId: produtoId };

        const dialogRef = this.dialog.open(ProdutosDetailComponent, dialogConfig);
        dialogRef.afterClosed().subscribe(
            () => {
                this.consultarProdutos();
            }
        );
    }

    atualizarStatus(produtoId: string): void {
        this.produtosService.atualizarStatus(produtoId).subscribe({
            next: () => {
                this.snackBar.open('Status atualizado com sucesso!', '', { duration: 2000 });
                this.consultarProdutos();
            },
            error: (err) => {
                this.snackBar.open(err, '', { duration: 2000 });
            }
        });
    }

    criarOferta(produtoId: string): void {
        const dialogConfig = new MatDialogConfig();
        dialogConfig.width = '900px';
        dialogConfig.disableClose = true;
        dialogConfig.data = { OfertaId: 'new', ProdutoId: produtoId };

        const dialogRef = this.dialog.open(OfertasDetailComponent, dialogConfig);
        dialogRef.afterClosed().subscribe(
            () => {
                this.consultarProdutos();
            }
        );
    }
}
