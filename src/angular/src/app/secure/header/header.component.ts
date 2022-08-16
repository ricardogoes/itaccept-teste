import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/_shared/services/auth.service';
import { Usuario } from '../empresas/models/usuarios.model';

@Component({
	selector: 'itaccept-header',
	templateUrl: 'header.component.html'
})
export class HeaderComponent implements OnInit {

    usuarioLogado: Usuario;
    constructor(
        private authService: AuthService,
        private router: Router
    ){
    }

	ngOnInit(): void {
        this.usuarioLogado = this.authService.getLoggedUserInfo();
	}

    sair(): void {
        sessionStorage.clear();
        this.router.navigate(['']);
    }
}
