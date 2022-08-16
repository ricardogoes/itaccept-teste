import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
    name: 'status'
})
export class StatusPipe implements PipeTransform{
    transform(value: boolean): string{
        if (value) {
            return 'Ativo';
        } else {
            return 'Arquivado';
        }
    }
}
