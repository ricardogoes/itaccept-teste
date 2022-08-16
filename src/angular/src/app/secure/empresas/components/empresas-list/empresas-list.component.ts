import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Title } from '@angular/platform-browser';
import { Observable, map, catchError, EMPTY, forkJoin } from 'rxjs';
import { Usuario } from 'src/app/secure/empresas/models/usuarios.model';
import { ApiResponse } from 'src/app/_shared/models/api-response.model';
import { AuthService } from 'src/app/_shared/services/auth.service';
import { Empresa } from '../../models/empresas.model';
import { EmbarcadorasService } from '../../services/embarcadoras.service';
import { TransportadorasService } from '../../services/transportadoras.service';
import { AssociarEmpresasComponent } from '../associar-empresas/associar-empresas.component';
import { EmpresasDetailComponent } from '../empresas-detail/empresas-detail.component';
import { UsuariosListComponent } from '../usuarios-list/usuarios-list.component';

@Component({
    selector: 'itaccept-empresas-list',
    templateUrl: 'empresas-list.component.html'
})
export class EmpresasListComponent implements OnInit {

    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatSort) set matSort(sort: MatSort) {
        this.dataSource.sort = sort;
    }

    usuarioLogado: Usuario;
    dataSource = new MatTableDataSource<Empresa>();
    colunas: string[] = ['NomeEmpresa', 'TipoEmpresa', 'Status', 'Acoes'];
    defaultFilterPredicate?: (data: any, filter: string) => boolean;

    empresas$: Observable<Empresa[]>;

    filtroDataSource: string;
    itemPesquisado: string;

    constructor(
        private titleService: Title,
        private authService: AuthService,
        private embarcadorasService: EmbarcadorasService,
        private transportadorasService: TransportadorasService,
        private snackBar: MatSnackBar,
        private dialog: MatDialog
    ) {
        this.titleService.setTitle('ItAccept | Empresas');
    }

    ngOnInit(): void {
        this.usuarioLogado = this.authService.getLoggedUserInfo();
        this.filtroDataSource = 'todos';
        this.consultarEmpresas();
    }

    consultarEmpresas(): void {
        this.empresas$ = forkJoin ([
            this.transportadorasService.consultarTransportadoras('todos'),
            this.embarcadorasService.consultarEmbarcadoras('todos')
        ]).pipe(
            map(empresas => {
                console.log(empresas);
                const empresasConcat = empresas[0].concat(empresas[1]);
                this.dataSource = new MatTableDataSource<Empresa>([...empresasConcat]);
                this.dataSource.paginator = this.paginator;
                this.defaultFilterPredicate = this.dataSource.filterPredicate;

                return empresasConcat;
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
                this.dataSource.filterPredicate = (data: Empresa, filter: string) => !filter || data.Status === (filter === 'ativos' ? true : false);
                this.dataSource.filter = filtro;
                break;
            }
            case 'arquivados': {
                this.dataSource.filterPredicate = (data: Empresa, filter: string) => !filter || data.Status === (filter === 'arquivados' ? false : true);
                this.dataSource.filter = filtro;
                break;
            }
        }
    }

    abrirDetalhes(empresa: Empresa): void {
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = true;
        dialogConfig.data = { EmpresaId: empresa.EmpresaId === 0 ? 'new' : empresa.EmpresaId, TipoEmpresa: empresa.TipoEmpresa };

        const dialogRef = this.dialog.open(EmpresasDetailComponent, dialogConfig);
        dialogRef.afterClosed().subscribe(
            () => {
                this.consultarEmpresas();
            }
        );
    }

    atualizarStatus(empresa: Empresa): void {
        if(empresa.TipoEmpresa === 'Embarcadora')
            this.atualizarStatusEmbarcadora(empresa);
        else
            this.atualizarStatusTransportadora(empresa);
    }

    atualizarStatusEmbarcadora(empresa: Empresa): void {
        this.embarcadorasService.atualizarStatus(empresa.EmpresaId).subscribe({
            next: () => {
                this.snackBar.open('Status atualizado com sucesso!', '', { duration: 2000 });
                this.consultarEmpresas();
            },
            error: (err) => {
                this.snackBar.open(err, '', { duration: 2000 });
            }
        });
    }

    atualizarStatusTransportadora(empresa: Empresa): void {
        this.transportadorasService.atualizarStatus(empresa.EmpresaId).subscribe({
            next: () => {
                this.snackBar.open('Status atualizado com sucesso!', '', { duration: 2000 });
                this.consultarEmpresas();
            },
            error: (err) => {
                this.snackBar.open(err, '', { duration: 2000 });
            }
        });
    }

    adicionarUsuarios(empresa: Empresa) {
        const dialogConfig = new MatDialogConfig();
        dialogConfig.width = '800px';
        dialogConfig.height = '600px';
        dialogConfig.disableClose = true;
        dialogConfig.data = { EmpresaId: empresa.EmpresaId, TipoEmpresa: empresa.TipoEmpresa };

        const dialogRef = this.dialog.open(UsuariosListComponent, dialogConfig);
        dialogRef.afterClosed().subscribe(
            () => {
                this.consultarEmpresas();
            }
        );
    }

    associarEmpresas(empresa: Empresa) {
        const dialogConfig = new MatDialogConfig();
        dialogConfig.width = '500px';
        dialogConfig.height = '600px';
        dialogConfig.disableClose = true;
        dialogConfig.data = { EmpresaId: empresa.EmpresaId, TipoEmpresa: empresa.TipoEmpresa };

        const dialogRef = this.dialog.open(AssociarEmpresasComponent, dialogConfig);
        dialogRef.afterClosed().subscribe(
            () => {
                this.consultarEmpresas();
            }
        );
    }
}
