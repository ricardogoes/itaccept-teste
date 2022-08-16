import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class ApiErrorHandler {
    constructor(private _router: Router) {}
    public count = 0;
    public handleError(error: any): string {
        var msg: string = '';

        if (error.status === 401 && this.count < 1) {
            alert('User not authenticated');
            this._router.navigate(['login']);
            this.count = this.count + 1;
            return '';
        }

        if (error.error.Data !== undefined || error.error.Data != null) msg = error.error.Data;

        if (error.error.Message !== undefined) {
            if (msg === undefined || msg === '' || msg === null) msg = error.error.Message;
            else msg = `${error.error.Message} : ${msg}`;
        }

        if (error.error.error != undefined && error.error.error != null) msg = error.error.error;

        if (msg == undefined) {
            var propriety = Object.keys(error.error);
            msg = error.error[propriety[0]][0];
        }

        return msg;
    }
}
