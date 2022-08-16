import { AbstractControl, AbstractControlOptions, ValidationErrors } from '@angular/forms';

export class PasswordValidator implements AbstractControlOptions {
    validators = (AC: AbstractControl): ValidationErrors | null => {
        let password = AC.get('Password')!.value; // to get value in input tag
        let confirmPassword = AC.get('ConfirmPassword')!.value; // to get value in input tag

        if (confirmPassword == null || password !== confirmPassword) AC.get('ConfirmPassword')!.setErrors({ MatchPassword: true });

        return null;
    };
}
