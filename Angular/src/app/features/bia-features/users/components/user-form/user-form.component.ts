import { NgIf, NgSwitch, NgSwitchCase } from '@angular/common';
import { Component } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { PrimeTemplate } from 'primeng/api';
import { BiaFormComponent } from 'src/app/shared/bia-shared/components/form/bia-form/bia-form.component';
import { CrudItemFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component';
import { User } from '../../model/user';
import { UserTeamsComponent } from '../user-teams/user-teams.component';

@Component({
  selector: 'bia-user-form',
  templateUrl: 'user-form.component.html',
  styleUrls: ['user-form.component.scss'],
  imports: [
    BiaFormComponent,
    NgSwitch,
    NgSwitchCase,
    NgIf,
    TranslateModule,
    UserTeamsComponent,
    UserTeamsComponent,
    PrimeTemplate,
    TranslateModule,
  ],
})
export class UserFormComponent extends CrudItemFormComponent<User> {}
