import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
    name: 'cpfCnpj'
})
export class CpfCnpjPipe implements PipeTransform{
    transform(value: string): string {
        let cpfCnpj = value.replace('-', '');
        cpfCnpj = cpfCnpj.replace('/', '');
        cpfCnpj = cpfCnpj.replace('.', '').replace('.', '');

        if (cpfCnpj.length === 11) {
            return cpfCnpj.substr(0, 3) + '.' + cpfCnpj.substr(3, 3) + '.' + cpfCnpj.substr(6, 3) + '-' + cpfCnpj.substr(9, 2);
        }

        if (cpfCnpj.length === 14) {
            return cpfCnpj.substr(0, 2) + '.' + cpfCnpj.substr(2, 3) + '.' + cpfCnpj.substr(5, 3) + '/' + cpfCnpj.substr(8, 4) + '-' + cpfCnpj.substr(12, 2);
        }

        return '';
    }
}
