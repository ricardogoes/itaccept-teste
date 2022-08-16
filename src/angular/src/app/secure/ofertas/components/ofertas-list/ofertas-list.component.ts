import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Title } from '@angular/platform-browser';
import { Observable, map, catchError, EMPTY } from 'rxjs';
import { Usuario } from 'src/app/secure/empresas/models/usuarios.model';
import { ApiResponse } from 'src/app/_shared/models/api-response.model';
import { AuthService } from 'src/app/_shared/services/auth.service';
import { Oferta } from '../../models/ofertas.model';
import { OfertasService } from '../../services/ofertas.service';
import { LancesListComponent } from '../lances-list/lances-list.component';
import { OfertasDetailComponent } from '../ofertas-detail/ofertas-detail.component';

@Component({
    selector: 'itaccept-ofertas-list',
    templateUrl: 'ofertas-list.component.html'
})
export class OfertasListComponent implements OnInit {
    usuarioLogado: Usuario;
    constructor(
        private titleService: Title,
        private authService: AuthService
    ) {
        this.titleService.setTitle('ItAccept | Ofertas');
    }

    ngOnInit(): void {
        this.usuarioLogado = this.authService.getLoggedUserInfo();
    }
}
