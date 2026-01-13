import { AsyncPipe, NgClass, NgStyle, NgTemplateOutlet } from '@angular/common';
import { Component } from '@angular/core';
import {
  FormsModule,
  ReactiveFormsModule,
  UntypedFormBuilder,
} from '@angular/forms';
import { AuthService, BiaMessageService } from '@bia-team/bia-ng/core';
import {
  BiaFrozenColumnDirective,
  BiaTableFilterComponent,
  BiaTableFooterControllerComponent,
  BiaTableInputComponent,
  BiaTableOutputComponent,
  CrudItemTableComponent,
} from '@bia-team/bia-ng/shared';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { PrimeTemplate } from 'primeng/api';
import { Skeleton } from 'primeng/skeleton';
import { TableModule } from 'primeng/table';
import { Tooltip } from 'primeng/tooltip';
import { Site } from '../../model/site';

@Component({
  selector: 'app-site-table',
  templateUrl:
    '../../../../../../node_modules/@bia-team/bia-ng/templates/components/table/bia-calc-table/bia-calc-table.component.html',
  styleUrls: [
    '../../../../../../node_modules/@bia-team/bia-ng/templates/components/table/bia-calc-table/bia-calc-table.component.scss',
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    TableModule,
    PrimeTemplate,
    Tooltip,
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
export class SiteTableComponent extends CrudItemTableComponent<Site> {
  constructor(
    public formBuilder: UntypedFormBuilder,
    public authService: AuthService,
    public biaMessageService: BiaMessageService,
    public translateService: TranslateService
  ) {
    super(formBuilder, authService, biaMessageService, translateService);
  }
}
