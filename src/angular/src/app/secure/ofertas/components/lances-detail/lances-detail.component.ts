import { AfterViewInit, Component, ElementRef, Inject, OnInit, ViewChildren } from '@angular/core';
import { FormControlName, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

import { FormBaseComponent } from 'src/app/_shared/components/form-base/form-base.component';
import { Lance } from '../../models/lances.model';
import { LancesService } from '../../services/lances.service';
import { catchError, EMPTY, map, Observable } from 'rxjs';
import { ApiResponse } from 'src/app/_shared/models/api-response.model';
import { Usuario } from 'src/app/secure/empresas/models/usuarios.model';
import { AuthService } from 'src/app/_shared/services/auth.service';
import { ProdutosService } from 'src/app/secure/produtos/services/produtos.service';
import { Produto } from 'src/app/secure/produtos/models/produtos.model';
import { Oferta } from '../../models/ofertas.model';

@Component({
    selector: 'itaccept-lances-detail',
    templateUrl: 'lances-detail.component.html'
})
export class LancesDetailComponent extends FormBaseComponent implements OnInit, AfterViewInit {
    @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];

    usuarioLogado: Usuario;

    lance$: Observable<Lance>;

    lanceForm: FormGroup;

    constructor(
        private fb: FormBuilder,
        private authService: AuthService,
        private lancesService: LancesService,
        private snackBar: MatSnackBar,
        private dialogRef: MatDialogRef<LancesDetailComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { LanceId: string, Oferta: Oferta }
    ) {
        super();

        const validationMessages = {
            Volume: {
                required: 'O volume é obrigatório',
                max: `O volume não pode ser maior que ${this.data.Oferta.Quantidade}`
            },
            Preco: {
                required: 'A preço é obrigatório'
            }
        };

        super.configurarMensagensValidacao(validationMessages);
    }

    ngOnInit(): void {
        this.usuarioLogado = this.authService.getLoggedUserInfo();

        this.criarForm();

        if (this.data.LanceId !== "new") {
            console.log(this.data);
            this.consultarLance();
        }
    }

    ngAfterViewInit(): void {
        super.configurarValidacoesFormulario(this.formInputElements, this.lanceForm);
    }

    consultarLance(): void {
        this.lance$ = this.lancesService.consultarPorId(this.data.LanceId).pipe(
            map(lance => {
                this.lanceForm.patchValue(lance);
                return lance;
            }),
            catchError(error => {
                this.snackBar.open(error, '', { duration: 2000 });
                return EMPTY
            })
        );
    }

    criarForm(): void {
        this.lanceForm = this.fb.group({
            Volume: ['', [Validators.required, Validators.max(this.data.Oferta.Quantidade)]],
            Preco: ['', [Validators.required]]
        });
    }

    salvar(): void {
        const lance = this.lanceForm.value;
        lance.TransportadoraId = this.usuarioLogado.EmpresaId;
        lance.OfertaId = this.data.Oferta.OfertaId;

        if (this.data.LanceId !== 'new') {
            this.atualizar(lance);
        } else {
            this.inserir(lance);
        }
    }

    atualizar(lance: Lance): void {
        lance.LanceId = parseInt(this.data.LanceId);
        this.lancesService.atualizar(lance).subscribe({
            next: () => {
                this.snackBar.open('Lance salvo com sucesso!', '', { duration: 2000 });
                this.dialogRef.close();
            },
            error: (err) => {
                this.snackBar.open(err, '', { duration: 2000 });
            }
        });
    }

    inserir(lance: Lance): void {
        this.lancesService.inserir(lance).subscribe({
            next: () => {
                this.snackBar.open('Lance salvo com sucesso!', '', { duration: 2000 });
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
        super.trimValue(eventTarget, this.lanceForm);
    }

    recognize(index, item) {
		return item
	}
}
