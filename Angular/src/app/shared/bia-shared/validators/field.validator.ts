import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export class FieldValidator {
  static atLeastOneFilled(fields: string[]): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      let isAtLeastOneFilled = false;

      for (const field of fields) {
        const fieldControl = control.get(field);

        if (fieldControl?.value) {
          isAtLeastOneFilled = true;
        }
      }

      // Setting each control errors and keeping errors from other validators
      fields.forEach(field => {
        const fieldControl = control.get(field);
        if (
          fieldControl &&
          isAtLeastOneFilled &&
          fieldControl.hasError('atLeastOneFilled')
        ) {
          const currentErrors = { ...fieldControl.errors };
          delete currentErrors['atLeastOneFilled'];
          fieldControl.setErrors(
            Object.keys(currentErrors).length === 0 ? null : currentErrors
          );
        } else if (
          fieldControl &&
          !isAtLeastOneFilled &&
          !fieldControl.hasError('atLeastOneFilled')
        ) {
          const currentErrors = { ...fieldControl.errors };
          currentErrors['atLeastOneFilled'] = true;
          fieldControl.setErrors(currentErrors);
        }
      });

      return isAtLeastOneFilled ? null : { atLeastOneFilled: true };
    };
  }
}
