import { AfterViewInit, Component, ElementRef, Inject, OnInit, ViewChildren } from '@angular/core';
import { FormControlName, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

import { FormBaseComponent } from 'src/app/_shared/components/form-base/form-base.component';
import { Empresa } from '../../models/empresas.model';
import { TransportadorasService } from '../../services/transportadoras.service';
import { EmbarcadorasService } from '../../services/embarcadoras.service';
import { catchError, EMPTY, map, Observable } from 'rxjs';
import { ApiResponse } from 'src/app/_shared/models/api-response.model';
import { Usuario } from 'src/app/secure/empresas/models/usuarios.model';
import { AuthService } from 'src/app/_shared/services/auth.service';

@Component({
    selector: 'itaccept-empresas-detail',
    templateUrl: 'empresas-detail.component.html'
})
export class EmpresasDetailComponent extends FormBaseComponent implements OnInit, AfterViewInit {
    @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];

    usuarioLogado: Usuario;

    empresa$: Observable<Empresa>;
    empresaForm: FormGroup;

    constructor(
        private fb: FormBuilder,
        private authService: AuthService,
        private transportadorasService: TransportadorasService,
        private embarcadorasService: EmbarcadorasService,
        private snackBar: MatSnackBar,
        private dialogRef: MatDialogRef<EmpresasDetailComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { EmpresaId: string, TipoEmpresa: string }
    ) {
        super();

        const validationMessages = {
            NomeEmpresa: {
                required: 'O nome da empresa é obrigatório'
            },
            TipoEmpresa: {
                required: 'O tipo do empresa é obrigatório'
            }
        };

        super.configurarMensagensValidacao(validationMessages);
    }

    ngOnInit(): void {
        this.usuarioLogado = this.authService.getLoggedUserInfo();

        this.criarForm();
        if (this.data.EmpresaId !== 'new') {
            this.consultarEmpresa();
        }
    }

    ngAfterViewInit(): void {
        super.configurarValidacoesFormulario(this.formInputElements, this.empresaForm);
    }

    consultarEmpresa(): void {
        if(this.data.TipoEmpresa === "Transportadora")
            this.empresa$ = this.consultarTransportadora();
        else
            this.empresa$ = this.consultarEmbarcadora();
    }

    consultarEmbarcadora(): Observable<Empresa> {
        return this.embarcadorasService.consultarPorId(this.data.EmpresaId).pipe(
            map(empresa => {
                this.empresaForm.patchValue(empresa);
                return empresa;
            }),
            catchError(error => {
                this.snackBar.open(error, '', { duration: 2000 });
                return EMPTY
            })
        );
    }

    consultarTransportadora(): Observable<Empresa> {
        return this.transportadorasService.consultarPorId(this.data.EmpresaId).pipe(
            map(empresa => {
                this.empresaForm.patchValue(empresa);
                return empresa;
            }),
            catchError(error => {
                this.snackBar.open(error, '', { duration: 2000 });
                return EMPTY
            })
        );
    }

    criarForm(): void {
        this.empresaForm = this.fb.group({
            NomeEmpresa: ['', [Validators.required]],
            TipoEmpresa: ['', [Validators.required]]
        });
    }

    salvar(): void {
        const empresa = this.empresaForm.value;
        empresa.EmpresaId = +this.data.EmpresaId;

        console.log(empresa);
        if (this.data.EmpresaId !== 'new') {
            this.atualizar(empresa);
        } else {
            this.inserir(empresa);
        }
    }

    atualizar(empresa: Empresa): void {
        empresa.EmpresaId = parseInt(this.data.EmpresaId);

        if(empresa.TipoEmpresa === 'Embarcadora')
            this.atualizarEmbarcadora(empresa);
        else
            this.atualizarTransportadora(empresa);
    }

    atualizarEmbarcadora(empresa: Empresa): void{
        this.embarcadorasService.atualizar(empresa).subscribe({
            next: (apiResponse: ApiResponse) => {
                this.snackBar.open('Empresa salvo com sucesso!', '', { duration: 2000 });
                this.dialogRef.close();
            },
            error: (err) => {
                this.snackBar.open(err, '', { duration: 2000 });
            }
        });
    }

    atualizarTransportadora(empresa: Empresa): void{
        this.transportadorasService.atualizar(empresa).subscribe({
            next: (apiResponse: ApiResponse) => {
                this.snackBar.open('Empresa salvo com sucesso!', '', { duration: 2000 });
                this.dialogRef.close();
            },
            error: (err) => {
                this.snackBar.open(err, '', { duration: 2000 });
            }
        });
    }

    inserir(empresa: Empresa): void {
        if(empresa.TipoEmpresa === 'Embarcadora')
            this.inserirEmbarcadora(empresa);
        else
            this.inserirTransportadora(empresa);
    }

    inserirEmbarcadora(empresa: Empresa): void {
        this.embarcadorasService.inserir(empresa).subscribe({
            next: (apiResponse: ApiResponse) => {
                this.snackBar.open('Empresa salvo com sucesso!', '', { duration: 2000 });
                this.dialogRef.close();
            },
            error: (err) => {
                this.snackBar.open(err, '', { duration: 2000 });
            }
        });
    }

    inserirTransportadora(empresa: Empresa): void {
        this.transportadorasService.inserir(empresa).subscribe({
            next: (apiResponse: ApiResponse) => {
                this.snackBar.open('Empresa salvo com sucesso!', '', { duration: 2000 });
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
        super.trimValue(eventTarget, this.empresaForm);
    }
}
