import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { ViewType } from '@bia-team/bia-ng/models/enum';

export class ViewValidator {
  static teamViewWithTeams(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const viewTypeControl = control.get('viewType');
      const viewTeamsControl = control.get('viewTeams');
      const fieldControls = [viewTypeControl, viewTeamsControl];
      let teamViewWithTeams = true;

      if (
        viewTypeControl?.value === ViewType.Team &&
        (!viewTeamsControl?.value || viewTeamsControl.value.length === 0)
      ) {
        teamViewWithTeams = false;
      }

      // Setting each control errors and keeping errors from other validators
      fieldControls.forEach(fieldControl => {
        if (
          fieldControl &&
          teamViewWithTeams &&
          fieldControl.hasError('teamViewWithTeams')
        ) {
          const currentErrors = { ...fieldControl.errors };
          delete currentErrors['teamViewWithTeams'];
          fieldControl.setErrors(
            Object.keys(currentErrors).length === 0 ? null : currentErrors
          );
        } else if (
          fieldControl &&
          !teamViewWithTeams &&
          !fieldControl.hasError('teamViewWithTeams')
        ) {
          const currentErrors = { ...fieldControl.errors };
          currentErrors['teamViewWithTeams'] = true;
          fieldControl.setErrors(currentErrors);
        }
      });

      return teamViewWithTeams ? null : { teamViewWithTeams: true };
    };
  }
}
