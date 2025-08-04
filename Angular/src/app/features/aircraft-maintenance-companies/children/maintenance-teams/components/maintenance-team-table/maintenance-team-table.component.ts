import {
  AsyncPipe,
  NgClass,
  NgFor,
  NgIf,
  NgStyle,
  NgSwitch,
  NgTemplateOutlet,
} from '@angular/common';
import { Component } from '@angular/core';
import {
  FormsModule,
  ReactiveFormsModule,
  UntypedFormBuilder,
} from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { AuthService, BiaMessageService } from 'bia-ng/core';
import {
  BiaFrozenColumnDirective,
  BiaTableFilterComponent,
  BiaTableFooterControllerComponent,
  BiaTableInputComponent,
  BiaTableOutputComponent,
  CrudItemTableComponent,
} from 'bia-ng/shared';
import { PrimeTemplate } from 'primeng/api';
import { Skeleton } from 'primeng/skeleton';
import { TableModule } from 'primeng/table';
import { Tooltip } from 'primeng/tooltip';
import { MaintenanceTeam } from '../../model/maintenance-team';

@Component({
  selector: 'app-maintenance-team-table',
  templateUrl:
    '../../../../../../../../node_modules/bia-ng/templates/components/table/bia-calc-table/bia-calc-table.component.html',
  styleUrls: [
    '../../../../../../../../node_modules/bia-ng/templates/components/table/bia-calc-table/bia-calc-table.component.scss',
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NgIf,
    TableModule,
    PrimeTemplate,
    NgFor,
    Tooltip,
    NgSwitch,
    NgClass,
    NgTemplateOutlet,
    Skeleton,
    NgStyle,
    AsyncPipe,
    TranslateModule,
    BiaTableFilterComponent,
    BiaTableInputComponent,
    BiaTableOutputComponent,
    BiaTableFooterControllerComponent,
    BiaFrozenColumnDirective,
  ],
})
export class MaintenanceTeamTableComponent extends CrudItemTableComponent<MaintenanceTeam> {
  constructor(
    public formBuilder: UntypedFormBuilder,
    public authService: AuthService,
    public biaMessageService: BiaMessageService,
    public translateService: TranslateService
  ) {
    super(formBuilder, authService, biaMessageService, translateService);
  }
}
