import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Observable, map, catchError, EMPTY, forkJoin } from 'rxjs';
import { Usuario } from 'src/app/secure/empresas/models/usuarios.model';
import { AuthService } from 'src/app/_shared/services/auth.service';
import { Lance } from '../../models/lances.model';
import { Oferta } from '../../models/ofertas.model';
import { LancesService } from '../../services/lances.service';
import { LancesDetailComponent } from '../lances-detail/lances-detail.component';

@Component({
    selector: 'itaccept-lances-list',
    templateUrl: 'lances-list.component.html'
})
export class LancesListComponent implements OnInit {

    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatSort) set matSort(sort: MatSort) {
        this.dataSource.sort = sort;
    }

    usuarioLogado: Usuario;
    dataSource = new MatTableDataSource<Lance>();
    colunas: string[] = ['NomeTransportadora', 'Volume', 'Preco', 'Status', 'LanceVencedor', 'Acoes'];
    defaultFilterPredicate?: (data: any, filter: string) => boolean;

    lances$: Observable<Lance[]>;

    filtroDataSource: string;
    itemPesquisado: string;

    constructor(
        private authService: AuthService,
        private lancesService: LancesService,
        private snackBar: MatSnackBar,
        private dialog: MatDialog,
        private dialogRef: MatDialogRef<LancesDetailComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { Oferta: Oferta }
    ) {}

    ngOnInit(): void {
        this.usuarioLogado = this.authService.getLoggedUserInfo();
        this.filtroDataSource = 'todos';
        this.consultarLances();
    }

    consultarLances(): void {
        this.lances$ = this.lancesService.consultarPelaOferta(this.data.Oferta.OfertaId, 'todos').pipe(
            map(lances => {
                this.dataSource = new MatTableDataSource<Lance>([...lances]);
                this.dataSource.paginator = this.paginator;
                this.defaultFilterPredicate = this.dataSource.filterPredicate;

                return lances;
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
                this.dataSource.filterPredicate = (data: Lance, filter: string) => !filter || data.Status === (filter === 'ativos' ? true : false);
                this.dataSource.filter = filtro;
                break;
            }
            case 'arquivados': {
                this.dataSource.filterPredicate = (data: Lance, filter: string) => !filter || data.Status === (filter === 'arquivados' ? false : true);
                this.dataSource.filter = filtro;
                break;
            }
        }
    }

    marcarComoVencedor(lanceId: number): void {
        this.lancesService.atualizarLanceVencedor(lanceId).subscribe({
            next: () => {
                this.snackBar.open('Lance atualizado com sucesso!', '', { duration: 2000 });
                this.consultarLances();
            },
            error: (err) => {
                this.snackBar.open(err, '', { duration: 2000 });
            }
        });
    }

    atualizarStatus(lance: Lance): void {
        this.lancesService.atualizarStatus(lance.LanceId).subscribe({
            next: () => {
                this.snackBar.open('Status atualizado com sucesso!', '', { duration: 2000 });
                this.consultarLances();
            },
            error: (err) => {
                this.snackBar.open(err, '', { duration: 2000 });
            }
        });
    }

    fechar(): void {
        this.dialogRef.close();
    }
}
