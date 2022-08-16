import { AfterViewInit, Component, ElementRef, Inject, OnInit, ViewChildren } from '@angular/core';
import { FormControlName, FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

import { FormBaseComponent } from 'src/app/_shared/components/form-base/form-base.component';
import { catchError, EMPTY, map, Observable } from 'rxjs';
import { ApiResponse } from 'src/app/_shared/models/api-response.model';
import { Usuario } from '../../models/usuarios.model';
import { UsuariosService } from '../../services/usuarios.service';
import { PasswordValidator } from '../../services/password-validator.service';
import { AuthService } from 'src/app/_shared/services/auth.service';

@Component({
    selector: 'itaccept-usuarios-detail',
    templateUrl: 'usuarios-detail.component.html'
})
export class UsuariosDetailComponent extends FormBaseComponent implements OnInit, AfterViewInit {
    @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];

    usuarioLogado: Usuario;

    usuario$: Observable<Usuario>;
    usuarioForm: FormGroup;

    passwordValidator = new PasswordValidator();

    constructor(
        private fb: FormBuilder,
        private authService: AuthService,
        private usuariosService: UsuariosService,
        private snackBar: MatSnackBar,
        private dialogRef: MatDialogRef<UsuariosDetailComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { EmpresaId: string, UsuarioId: string }
    ) {
        super();

        const validationMessages = {
            NomeUsuario: {
                required: 'O nome do usuario é obrigatório'
            },
            Username: {
                required: 'O username é obrigatório'
            },
            Password: {
                required: 'O password é obrigatório',
                pattern: 'O password deve conter pelo menos 8 caracteres, sendo pelo menos 1 maiúscula, 1 mínuscula, números e caracteres especiais',
            },
            ConfirmPassword: {
                required: 'A confirmação do password é obrigatório'
            }
        };

        super.configurarMensagensValidacao(validationMessages);
    }

    ngOnInit(): void {
        this.usuarioLogado = this.authService.getLoggedUserInfo();
        this.criarForm();
        if (this.data.UsuarioId !== 'new') {
            this.consultarUsuario();
        }
    }

    ngAfterViewInit(): void {
        super.configurarValidacoesFormulario(this.formInputElements, this.usuarioForm);
    }

    consultarUsuario(): void {
        this.usuario$ = this.usuariosService.consultarPorId(this.data.UsuarioId).pipe(
            map(usuario => {
                this.usuarioForm.patchValue(usuario);
                return usuario;
            }),
            catchError(error => {
                this.snackBar.open(error, '', { duration: 2000 });
                return EMPTY
            })
        );
    }

    conditionalRequired() {
        return (control: FormControl): { [s: string]: boolean } => {
            let validate: boolean = false;

            if (this.data.UsuarioId === 'new' && (control.value === '' || control.value === null)) validate = true;

            if (validate) return { required: validate };

            return {};
        };
    }

    criarForm(): void {
        this.usuarioForm = this.fb.group({
            NomeUsuario: ['', [Validators.required]],
            Username: ['', [Validators.required]],
            Password: ['', [this.conditionalRequired(), Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,12})/)]],
            ConfirmPassword: ['']
        },
        this.passwordValidator);
    }

    salvar(): void {
        const usuario = this.usuarioForm.value;
        usuario.UsuarioId = +this.data.UsuarioId;
        usuario.EmpresaId = +this.data.EmpresaId;
        usuario.TipoUsuario = 'Funcionario'

        console.log(usuario);
        if (this.data.UsuarioId !== 'new') {
            this.atualizar(usuario);
        } else {
            this.inserir(usuario);
        }
    }

    atualizar(usuario: Usuario): void {
        this.usuariosService.atualizar(usuario).subscribe({
            next: () => {
                this.snackBar.open('Usuario salvo com sucesso!', '', { duration: 2000 });
                this.dialogRef.close();
            },
            error: (err) => {
                this.snackBar.open(err, '', { duration: 2000 });
            }
        });
    }

    inserir(usuario: Usuario): void {
        this.usuariosService.inserir(usuario).subscribe({
            next: () => {
                this.snackBar.open('Usuario salvo com sucesso!', '', { duration: 2000 });
                this.dialogRef.close();
            },
            error: (err) => {
                this.snackBar.open(err, '', { duration: 2000 });
            }
        });
    }

    fechar(): void {
        this.dialogRef.close();
    }

    trim(eventTarget: any): void {
        super.trimValue(eventTarget, this.usuarioForm);
    }
}
