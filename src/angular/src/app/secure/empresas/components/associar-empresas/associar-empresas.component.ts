import { ThisReceiver } from '@angular/compiler';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, EMPTY, forkJoin, map, Observable } from 'rxjs';
import { EmbarcadoraTransportadora } from '../../models/embarcadoras-transportadoras.model';
import { Empresa } from '../../models/empresas.model';
import { EmbarcadorasTransportadorasService } from '../../services/embarcadoras-transportadoras.service';
import { EmbarcadorasService } from '../../services/embarcadoras.service';
import { TransportadorasService } from '../../services/transportadoras.service';

@Component({
    selector: 'itaccept-associar-empresas',
    templateUrl: './associar-empresas.component.html'
})
export class AssociarEmpresasComponent implements OnInit {
    data$: Observable<any>;

    filtroDataSource: string;
    itemPesquisado: string;

    embarcadoraTransportadora: EmbarcadoraTransportadora;

    empresas: Empresa[];
    empresasFiltradas: Empresa[];
    empresasAssociadas: Empresa[];
    empresasNaoAssociadas: Empresa[];

    constructor(
        private embarcadorasTransportadorasService: EmbarcadorasTransportadorasService,
        private embarcadorasService: EmbarcadorasService,
        private transportadorasService: TransportadorasService,
        private snackBar: MatSnackBar,
        private dialogRef: MatDialogRef<AssociarEmpresasComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { EmpresaId: string, TipoEmpresa: string }
    ) { }

    ngOnInit(): void {
        this.filtroDataSource = 'todos';
        this.embarcadoraTransportadora = new EmbarcadoraTransportadora();
        this.carregarDadosAssociados();
    }

    aplicarFiltro(itemPesquisado: string): void {
		this.empresasFiltradas = this.empresas.filter(x => x.NomeEmpresa.toLowerCase().includes(itemPesquisado.toLowerCase()))
	}

    carregarDadosAssociados(): void {
        if (this.data.TipoEmpresa === 'Embarcadora')
            this.carregarTransportadoras();
        else
            this.carregarEmbarcadoras();
    }

    carregarEmbarcadoras(): void {
        this.data$ = forkJoin([
            this.embarcadorasService.consultarPorTransportadora(this.data.EmpresaId, 'associados'),
            this.embarcadorasService.consultarPorTransportadora(this.data.EmpresaId, 'nao-associados')
        ]).pipe(
            map(results => {
                this.empresasAssociadas = [...results[0]];
                this.empresasNaoAssociadas = [...results[1]];
                this.empresas = this.empresasAssociadas.concat(this.empresasNaoAssociadas).sort((a, b) => a.NomeEmpresa.localeCompare(b.NomeEmpresa));
                this.empresasFiltradas = [...this.empresas];

                return results;
            }),
            catchError(error => {
                this.snackBar.open(error, '', { duration: 2000 });
                return EMPTY
            })
        );
    }

    carregarTransportadoras(): void {
        this.data$ = forkJoin([
            this.transportadorasService.consultarPorEmbarcadora(this.data.EmpresaId, 'associados'),
            this.transportadorasService.consultarPorEmbarcadora(this.data.EmpresaId, 'nao-associados')
        ]).pipe(
            map(results => {
                this.empresasAssociadas = [...results[0]];
                this.empresasNaoAssociadas = [...results[1]];
                this.empresas = this.empresasAssociadas.concat(this.empresasNaoAssociadas).sort((a, b) => a.NomeEmpresa.localeCompare(b.NomeEmpresa));
                this.empresasFiltradas = [...this.empresas];

                return results;
            }),
            catchError(error => {
                this.snackBar.open(error, '', { duration: 2000 });
                return EMPTY
            })
        );
    }

    associarEmpresa(empresa: Empresa, isChecked: boolean): void {
        if (isChecked) {
            this.adicionar(empresa.EmpresaId);
        } else {
            this.apagar(empresa.EmpresaId);
        }
    }

    adicionar(empresaId: number): void {
        if (this.data.TipoEmpresa === 'Embarcadora') {
            this.embarcadoraTransportadora.EmbarcadoraId = +this.data.EmpresaId;
            this.embarcadoraTransportadora.TransportadoraId = empresaId;
        } else {
            this.embarcadoraTransportadora.EmbarcadoraId = empresaId;
            this.embarcadoraTransportadora.TransportadoraId = +this.data.EmpresaId;
        }

        this.embarcadorasTransportadorasService.inserir(this.embarcadoraTransportadora).subscribe({
            next: () => {
                this.snackBar.open('Associação criada com sucesso!', '', { duration: 2000 });

                this.empresasAssociadas.push(this.empresas.find(x => x.EmpresaId === empresaId)!);

                const indice = this.empresasNaoAssociadas.indexOf(this.empresasNaoAssociadas.find(x => x.EmpresaId === empresaId)!);
                this.empresasNaoAssociadas.splice(indice, 1);

                this.filtrarDataSource(this.filtroDataSource);
            },
            error: (err) => {
                this.snackBar.open(err, '', { duration: 2000 });
            }
        });
    }

    apagar(empresaId: number): void {
        let embarcadoraId, transportadoraId;
        if (this.data.TipoEmpresa === 'Embarcadora') {
            embarcadoraId = +this.data.EmpresaId;
            transportadoraId = empresaId;
        } else {
            embarcadoraId = empresaId;
            transportadoraId = +this.data.EmpresaId
        }


        this.embarcadorasTransportadorasService.apagar(embarcadoraId, transportadoraId).subscribe({
            next: () => {
                this.snackBar.open('Associação excluída com sucesso!', '', { duration: 2000 });

                this.empresasNaoAssociadas.push(this.empresas.find(x => x.EmpresaId === empresaId)!);

                const indice = this.empresasAssociadas.indexOf(this.empresasAssociadas.find(x => x.EmpresaId === empresaId)!);
                this.empresasAssociadas.splice(indice, 1);

                this.filtrarDataSource(this.filtroDataSource);
            },
            error: (err) => {
                this.snackBar.open(err, '', { duration: 2000 });
            }
        });
    }

    fechar(): void {
        this.dialogRef.close();
    }

    filtrarDataSource(filtro: string): void {
        this.filtroDataSource = filtro;

        this.itemPesquisado = '';

        if (filtro === 'todos') {
            this.empresasFiltradas = this.empresas.sort((a, b) => a.NomeEmpresa.localeCompare(b.NomeEmpresa));
        } else if (filtro === 'associados') {
            this.empresasFiltradas = this.empresasAssociadas.sort((a, b) => a.NomeEmpresa.localeCompare(b.NomeEmpresa));
        } else if (filtro === 'nao-associados') {
            this.empresasFiltradas = this.empresasNaoAssociadas.sort((a, b) => a.NomeEmpresa.localeCompare(b.NomeEmpresa));
        }
    }

    usuarioEstaAssociadoAoCliente(empresa: Empresa): boolean {
        return this.empresasAssociadas.find(x => x.EmpresaId === empresa.EmpresaId) !== undefined;
    }

    recognize(index, item) {
        return item
    }
}
