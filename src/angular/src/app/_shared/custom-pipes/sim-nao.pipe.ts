import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
    name: 'simNao'
})
export class SimNaoPipe implements PipeTransform{
    transform(value: boolean): string{
        if (value) {
            return 'Sim';
        } else {
            return 'Não';
        }
    }
}
