import { AbstractControl, ValidationErrors } from '@angular/forms';

export class JsonValidator {
  static valid(control: AbstractControl): ValidationErrors | null {
    if (control.value) {
      try {
        JSON.parse(control.value);
      } catch {
        return { jsonInvalid: true };
      }
    }

    return null;
  }
}
