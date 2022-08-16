import { AfterViewInit, Component, ElementRef, Inject, OnInit, ViewChildren } from '@angular/core';
import { FormControlName, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

import { FormBaseComponent } from 'src/app/_shared/components/form-base/form-base.component';
import { Produto } from '../../models/produtos.model';
import { ProdutosService } from '../../services/produtos.service';
import { catchError, EMPTY, map, Observable } from 'rxjs';
import { ApiResponse } from 'src/app/_shared/models/api-response.model';
import { Usuario } from 'src/app/secure/empresas/models/usuarios.model';
import { AuthService } from 'src/app/_shared/services/auth.service';

@Component({
    selector: 'itaccept-produtos-detail',
    templateUrl: 'produtos-detail.component.html'
})
export class ProdutosDetailComponent extends FormBaseComponent implements OnInit, AfterViewInit {
    @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];

    usuarioLogado: Usuario;

    produto$: Observable<Produto>;
    produtoForm: FormGroup;

    constructor(
        private fb: FormBuilder,
        private authService: AuthService,
        private produtosService: ProdutosService,
        private snackBar: MatSnackBar,
        private dialogRef: MatDialogRef<ProdutosDetailComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { ProdutoId: string }
    ) {
        super();

        const validationMessages = {
            NomeProduto: {
                required: 'O nome do produto é obrigatório'
            }
        };

        super.configurarMensagensValidacao(validationMessages);
    }

    ngOnInit(): void {
        this.usuarioLogado = this.authService.getLoggedUserInfo();
        this.criarForm();

        if (this.data.ProdutoId !== "new") {
            this.consultarProduto();
        }
    }

    ngAfterViewInit(): void {
        super.configurarValidacoesFormulario(this.formInputElements, this.produtoForm);
    }

    consultarProduto(): void {
        this.produto$ = this.produtosService.consultarPorId(this.data.ProdutoId).pipe(
            map(produto => {
                this.produtoForm.patchValue(produto);
                return produto;
            }),
            catchError(error => {
                this.snackBar.open(error, '', { duration: 2000 });
                return EMPTY
            })
        );
    }

    criarForm(): void {
        this.produtoForm = this.fb.group({
            NomeProduto: ['', [Validators.required]]
        });
    }

    salvar(): void {
        const produto = this.produtoForm.value;
        produto.ProdutoId = +this.data.ProdutoId;
        produto.EmbarcadoraId = this.usuarioLogado.EmpresaId;

        if (this.data.ProdutoId !== 'new') {
            this.atualizar(produto);
        } else {
            this.inserir(produto);
        }
    }

    atualizar(produto: Produto): void {
        produto.ProdutoId = parseInt(this.data.ProdutoId);
        this.produtosService.atualizar(produto).subscribe({
            next: () => {
                this.snackBar.open('Produto salvo com sucesso!', '', { duration: 2000 });
                this.dialogRef.close();
            },
            error: (err) => {
                this.snackBar.open(err, '', { duration: 2000 });
            }
        });
    }

    inserir(produto: Produto): void {
        this.produtosService.inserir(produto).subscribe({
            next: () => {
                this.snackBar.open('Produto salvo com sucesso!', '', { duration: 2000 });
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
        super.trimValue(eventTarget, this.produtoForm);
    }
}
