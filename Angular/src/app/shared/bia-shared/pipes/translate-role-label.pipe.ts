import { Pipe, PipeTransform } from '@angular/core';
import { Role } from 'src/app/domains/role/model/role';

@Pipe({
  name: 'translateRoleLabel'
})
export class TranslateRoleLabelPipe implements PipeTransform {

  transform(role: Role, currentLang: string): any {
    if (!role) {
      return undefined;
    }

    let roleLabel = '';
    switch (currentLang) {
      case 'fr':
        roleLabel = role.labelFr;
        break;
      case 'es':
        roleLabel = role.labelEs;
        break;
    }

    return roleLabel && roleLabel.length > 0 ? roleLabel : role.labelEn;
  }
}
