import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Title } from '@angular/platform-browser';
import { Observable, map, catchError, EMPTY } from 'rxjs';
import { Usuario } from 'src/app/secure/empresas/models/usuarios.model';
import { AuthService } from 'src/app/_shared/services/auth.service';
import { Oferta } from '../../models/ofertas.model';
import { OfertasService } from '../../services/ofertas.service';
import { LancesListComponent } from '../lances-list/lances-list.component';
import { OfertasDetailComponent } from '../ofertas-detail/ofertas-detail.component';

@Component({
    selector: 'itaccept-ofertas-list-embarcadora',
    templateUrl: 'ofertas-list-embarcadora.component.html'
})
export class OfertasListEmbarcadoraComponent implements OnInit {

    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatSort) set matSort(sort: MatSort) {
        this.dataSource.sort = sort;
    }

    usuarioLogado: Usuario;
    dataSource = new MatTableDataSource<Oferta>();
    colunas: string[] = ['NomeProduto', 'Quantidade', 'QuantidadeDisponivel', 'EnderecoOrigem', 'EnderecoDestino', 'StatusOferta','Status', 'Acoes'];
    defaultFilterPredicate?: (data: any, filter: string) => boolean;

    ofertas$: Observable<Oferta[]>;

    filtroDataSource: string;
    itemPesquisado: string;

    constructor(
        private titleService: Title,
        private authService: AuthService,
        private ofertasService: OfertasService,
        private snackBar: MatSnackBar,
        private dialog: MatDialog
    ) {
        this.titleService.setTitle('ItAccept | Ofertas');
    }

    ngOnInit(): void {
        this.usuarioLogado = this.authService.getLoggedUserInfo();
        this.filtroDataSource = 'todos';

        this.consultarOfertasPelaEmbarcadora();
    }

    consultarOfertasPelaEmbarcadora(): void {
        this.ofertas$ = this.ofertasService.consultarPorEmbarcadora(this.usuarioLogado.EmpresaId, 'todos').pipe(
            map(ofertas => {
                this.dataSource = new MatTableDataSource<Oferta>([...ofertas]);
                this.dataSource.paginator = this.paginator;
                this.defaultFilterPredicate = this.dataSource.filterPredicate;

                return ofertas;
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
                this.dataSource.filterPredicate = (data: Oferta, filter: string) => !filter || data.Status === (filter === 'ativos' ? true : false);
                this.dataSource.filter = filtro;
                break;
            }
            case 'arquivados': {
                this.dataSource.filterPredicate = (data: Oferta, filter: string) => !filter || data.Status === (filter === 'arquivados' ? false : true);
                this.dataSource.filter = filtro;
                break;
            }
        }
    }

    abrirDetalhes(ofertaId: string): void {
        const dialogConfig = new MatDialogConfig();
        dialogConfig.width = '900px';
        dialogConfig.disableClose = true;
        dialogConfig.data = { OfertaId: ofertaId, ProdutoId: '' };

        const dialogRef = this.dialog.open(OfertasDetailComponent, dialogConfig);
        dialogRef.afterClosed().subscribe(
            () => {
                this.consultarOfertasPelaEmbarcadora();
            }
        );
    }

    atualizarStatus(ofertaId: number): void {
        this.ofertasService.atualizarStatus(ofertaId).subscribe({
            next: () => {
                this.snackBar.open('Status atualizado com sucesso!', '', { duration: 2000 });
                this.consultarOfertasPelaEmbarcadora();
            },
            error: (err) => {
                this.snackBar.open(err, '', { duration: 2000 });
            }
        });
    }

    abrirLances(oferta: Oferta): void {
        const dialogConfig = new MatDialogConfig();
        dialogConfig.width = '800px';
        dialogConfig.height = '600px';
        dialogConfig.disableClose = true;
        dialogConfig.data = { Oferta: oferta };

        const dialogRef = this.dialog.open(LancesListComponent, dialogConfig);
        dialogRef.afterClosed().subscribe(
            () => {
                this.consultarOfertasPelaEmbarcadora();
            }
        );
    }
}
