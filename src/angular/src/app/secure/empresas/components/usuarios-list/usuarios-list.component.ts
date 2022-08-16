import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Observable, map, catchError, EMPTY, forkJoin } from 'rxjs';
import { AuthService } from 'src/app/_shared/services/auth.service';
import { Usuario } from '../../models/usuarios.model';
import { UsuariosService } from '../../services/usuarios.service';
import { UsuariosDetailComponent } from '../usuarios-detail/usuarios-detail.component';

@Component({
    selector: 'itaccept-usuarios-list',
    templateUrl: 'usuarios-list.component.html'
})
export class UsuariosListComponent implements OnInit {

    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatSort) set matSort(sort: MatSort) {
        this.dataSource.sort = sort;
    }

    usuarioLogado: Usuario;
    dataSource = new MatTableDataSource<Usuario>();
    colunas: string[] = ['NomeUsuario', 'Status', 'Acoes'];
    defaultFilterPredicate?: (data: any, filter: string) => boolean;

    usuarios$: Observable<Usuario[]>;

    filtroDataSource: string;
    itemPesquisado: string;

    constructor(
        private authService: AuthService,
        private usuariosService: UsuariosService,
        private snackBar: MatSnackBar,
        private dialog: MatDialog,
        private dialogRef: MatDialogRef<UsuariosDetailComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { EmpresaId: number, TipoEmpresa: string }
    ) {}

    ngOnInit(): void {
        this.usuarioLogado = this.authService.getLoggedUserInfo();
        this.filtroDataSource = 'todos';
        this.consultarUsuarios();
    }

    consultarUsuarios(): void {
        this.usuarios$ = this.usuariosService.consultarPorEmbarcadora(this.data.EmpresaId, 'todos').pipe(
            map(usuarios => {
                this.dataSource = new MatTableDataSource<Usuario>([...usuarios]);
                this.dataSource.paginator = this.paginator;
                this.defaultFilterPredicate = this.dataSource.filterPredicate;

                return usuarios;
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
                this.dataSource.filterPredicate = (data: Usuario, filter: string) => !filter || data.Status === (filter === 'ativos' ? true : false);
                this.dataSource.filter = filtro;
                break;
            }
            case 'arquivados': {
                this.dataSource.filterPredicate = (data: Usuario, filter: string) => !filter || data.Status === (filter === 'arquivados' ? false : true);
                this.dataSource.filter = filtro;
                break;
            }
        }
    }

    abrirDetalhes(usuarioId: string): void {
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = true;
        dialogConfig.data = { UsuarioId: usuarioId, EmpresaId: this.data.EmpresaId };

        const dialogRef = this.dialog.open(UsuariosDetailComponent, dialogConfig);
        dialogRef.afterClosed().subscribe(
            () => {
                this.consultarUsuarios();
            }
        );
    }

    atualizarStatus(usuario: Usuario): void {
        this.usuariosService.atualizarStatus(usuario.UsuarioId).subscribe({
            next: () => {
                this.snackBar.open('Status atualizado com sucesso!', '', { duration: 2000 });
                this.consultarUsuarios();
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
