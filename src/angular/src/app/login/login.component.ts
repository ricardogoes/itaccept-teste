import { AfterViewInit, Component, ElementRef, OnInit, ViewChildren, ViewEncapsulation } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControlName } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { AuthService } from 'src/app/_shared/services/auth.service';
import { Login } from './models/login.model';
import { Usuario } from 'src/app/secure/empresas/models/usuarios.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormBaseComponent } from 'src/app/_shared/components/form-base/form-base.component';
import { Title } from '@angular/platform-browser';

@Component({
    selector: 'itaccept-login',
    templateUrl: 'login.component.html',
    encapsulation: ViewEncapsulation.None
})
export class LoginComponent extends FormBaseComponent implements OnInit, AfterViewInit {
    @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];

    loginForm: FormGroup;

    sessaoExpirada: string;
    returnUrl: string;

    userLogin: Login;
    usuarioLogado: Usuario;

    error: boolean;
    constructor(
        private titleService: Title,
        private fb: FormBuilder,
        private router: Router,
        private route: ActivatedRoute,
        private authService: AuthService,
        private snackBar: MatSnackBar
    ) {
        super();

        this.titleService.setTitle('ItAccept | Login');

        sessionStorage.clear();

        const validationMessages = {
            Username: {
                required: 'O username é obrigatório'
            },
            Password: {
                required: 'A senha é obrigatória'
            }
        };

        super.configurarMensagensValidacao(validationMessages);

    }

    ngOnInit(): void {
        this.userLogin = new Login();
        this.criarFormLogin();
    }

    ngAfterViewInit(): void {
        super.configurarValidacoesFormulario(this.formInputElements, this.loginForm);
    }

    criarFormLogin(): void {
        this.loginForm = this.fb.group({
            Username: [this.userLogin.Username, [Validators.required]],
            Password: [this.userLogin.Password, [Validators.required]]
        });
    }

    autenticar(): void {
        this.error = false;
        this.authService.logout();

        this.userLogin.Username = this.loginForm.controls['Username'].value;
        this.userLogin.Password = this.loginForm.controls['Password'].value;
        this.authService.login(this.userLogin.Username, this.userLogin.Password).subscribe({
            next: () => {
                this.usuarioLogado = this.authService.getLoggedUserInfo();
                if(this.usuarioLogado.TipoUsuario === 'Admin')
                    this.router.navigate(['secure/empresas']);
                else
                    this.router.navigate(['secure/ofertas']);
            },
            error: () => {
                this.error = true;
            }
        });
    }
}
