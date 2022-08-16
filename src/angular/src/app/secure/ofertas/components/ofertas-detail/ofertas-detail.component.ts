import { AfterViewInit, Component, ElementRef, Inject, OnInit, ViewChildren } from '@angular/core';
import { FormControlName, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

import { FormBaseComponent } from 'src/app/_shared/components/form-base/form-base.component';
import { Oferta } from '../../models/ofertas.model';
import { OfertasService } from '../../services/ofertas.service';
import { catchError, EMPTY, map, Observable } from 'rxjs';
import { ApiResponse } from 'src/app/_shared/models/api-response.model';
import { Usuario } from 'src/app/secure/empresas/models/usuarios.model';
import { AuthService } from 'src/app/_shared/services/auth.service';
import { ProdutosService } from 'src/app/secure/produtos/services/produtos.service';
import { Produto } from 'src/app/secure/produtos/models/produtos.model';

@Component({
    selector: 'itaccept-ofertas-detail',
    templateUrl: 'ofertas-detail.component.html'
})
export class OfertasDetailComponent extends FormBaseComponent implements OnInit, AfterViewInit {
    @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];

    usuarioLogado: Usuario;

    oferta$: Observable<Oferta>;
    produtos$: Observable<Produto[]>;

    ofertaForm: FormGroup;

    constructor(
        private fb: FormBuilder,
        private authService: AuthService,
        private produtosService: ProdutosService,
        private ofertasService: OfertasService,
        private snackBar: MatSnackBar,
        private dialogRef: MatDialogRef<OfertasDetailComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { OfertaId: string, ProdutoId: string }
    ) {
        super();

        const validationMessages = {
            ProdutoId: {
                required: 'O produto é obrigatório'
            },
            Quantidade: {
                required: 'A quantidade é obrigatória'
            },
            EnderecoOrigem: {
                required: 'O endereço de origem é obrigatório'
            },
            EnderecoDestino: {
                required: 'O endereço de destino é obrigatório'
            }
        };

        super.configurarMensagensValidacao(validationMessages);
    }

    ngOnInit(): void {
        this.usuarioLogado = this.authService.getLoggedUserInfo();

        this.consultarProdutos();

        this.criarForm();

        if (this.data.OfertaId !== "new" || this.data.ProdutoId !== '') {
            console.log(this.data);
            this.consultarOferta();
        }
    }

    ngAfterViewInit(): void {
        super.configurarValidacoesFormulario(this.formInputElements, this.ofertaForm);
    }

    consultarOferta(): void {
        this.oferta$ = this.ofertasService.consultarPorId(this.data.OfertaId).pipe(
            map(oferta => {
                this.ofertaForm.patchValue(oferta);
                return oferta;
            }),
            catchError(error => {
                this.snackBar.open(error, '', { duration: 2000 });
                return EMPTY
            })
        );
    }

    consultarProdutos(): void {
        this.produtos$ = this.produtosService.consultarPorEmbarcadora(this.usuarioLogado.EmpresaId, 'ativos').pipe(
            map(produtos => {
                return produtos;
            }),
            catchError(error => {
                this.snackBar.open(error, '', { duration: 2000 });
                return EMPTY
            })
        );
    }

    criarForm(): void {
        this.ofertaForm = this.fb.group({
            ProdutoId: ['', [Validators.required]],
            Quantidade: ['', [Validators.required]],
            EnderecoOrigem: ['', [Validators.required]],
            EnderecoDestino: ['', [Validators.required]],
        });
    }

    salvar(): void {
        const oferta = this.ofertaForm.value;
        oferta.EmbarcadoraId = this.usuarioLogado.EmpresaId;

        if (this.data.OfertaId !== 'new') {
            this.atualizar(oferta);
        } else {
            this.inserir(oferta);
        }
    }

    atualizar(oferta: Oferta): void {
        oferta.OfertaId = parseInt(this.data.OfertaId);
        this.ofertasService.atualizar(oferta).subscribe({
            next: () => {
                this.snackBar.open('Oferta salvo com sucesso!', '', { duration: 2000 });
                this.dialogRef.close();
            },
            error: (err) => {
                this.snackBar.open(err, '', { duration: 2000 });
            }
        });
    }

    inserir(oferta: Oferta): void {
        this.ofertasService.inserir(oferta).subscribe({
            next: () => {
                this.snackBar.open('Oferta salvo com sucesso!', '', { duration: 2000 });
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
        super.trimValue(eventTarget, this.ofertaForm);
    }

    recognize(index, item) {
		return item
	}
}
