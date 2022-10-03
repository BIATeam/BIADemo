import {
  Component,
} from '@angular/core';
import { CrudItemFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component';
import { User } from '../../model/user';

@Component({
  selector: 'bia-user-form',
  templateUrl: '../../../../../shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.html',
  styleUrls: ['../../../../../shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss'],
})

export class UserFormComponent extends CrudItemFormComponent<User> {
}

