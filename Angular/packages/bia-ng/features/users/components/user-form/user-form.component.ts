import { NgIf, NgSwitch, NgSwitchCase } from '@angular/common';
import { Component } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { BiaFormComponent, CrudItemFormComponent } from 'bia-ng/shared';
import { PrimeTemplate } from 'primeng/api';
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
