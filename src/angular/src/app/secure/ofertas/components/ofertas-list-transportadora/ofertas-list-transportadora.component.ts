import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Title } from '@angular/platform-browser';
import { Observable, map, catchError, EMPTY } from 'rxjs';
import { Usuario } from 'src/app/secure/empresas/models/usuarios.model';
import { AuthService } from 'src/app/_shared/services/auth.service';
import { Oferta } from '../../models/ofertas.model';
import { OfertasService } from '../../services/ofertas.service';
import { LancesDetailComponent } from '../lances-detail/lances-detail.component';
import { OfertasDetailComponent } from '../ofertas-detail/ofertas-detail.component';

@Component({
    selector: 'itaccept-ofertas-list-transportadora',
    templateUrl: 'ofertas-list-transportadora.component.html'
})
export class OfertasListTransportadoraComponent implements OnInit {
    usuarioLogado: Usuario;

    ofertas$: Observable<Oferta[]>;
    ofertas: Oferta[];

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
        this.consultarOfertasPelaTransportadora();
    }

    consultarOfertasPelaTransportadora(): void {
        this.ofertas$ = this.ofertasService.consultarPorTransportadora(this.usuarioLogado.EmpresaId).pipe(
            map(ofertas => {
                this.ofertas = ofertas;
                return ofertas;
            }),
            catchError(error => {
                this.snackBar.open(error, '', { duration: 2000 });
                return EMPTY
            })
        );
    }

    abrirDetalhes(ofertaId: string): void {
        const dialogConfig = new MatDialogConfig();
        dialogConfig.width = '900px';
        dialogConfig.disableClose = true;
        dialogConfig.data = { OfertaId: ofertaId, ProdutoId: '' };

        const dialogRef = this.dialog.open(OfertasDetailComponent, dialogConfig);
        dialogRef.afterClosed().subscribe(
            () => {
                this.consultarOfertasPelaTransportadora();
            }
        );
    }

    atualizarStatus(ofertaId: number): void {
        this.ofertasService.atualizarStatus(ofertaId).subscribe({
            next: () => {
                this.snackBar.open('Status atualizado com sucesso!', '', { duration: 2000 });
                this.consultarOfertasPelaTransportadora();
            },
            error: (err) => {
                this.snackBar.open(err, '', { duration: 2000 });
            }
        });
    }

    darLance(oferta: Oferta): void {
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = true;
        dialogConfig.data = { LanceId: 'new', Oferta: oferta };

        const dialogRef = this.dialog.open(LancesDetailComponent, dialogConfig);
        dialogRef.afterClosed().subscribe(
            () => {
                this.consultarOfertasPelaTransportadora();
            }
        );
    }

    recognize(index, item) {
        return item
    }
}
