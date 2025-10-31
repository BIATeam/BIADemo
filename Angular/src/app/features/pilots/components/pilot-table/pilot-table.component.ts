import { AsyncPipe, NgClass, NgStyle, NgTemplateOutlet } from '@angular/common';
import { Component } from '@angular/core';
import {
  FormsModule,
  ReactiveFormsModule,
  UntypedFormBuilder,
} from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import {
  AuthService,
  BiaMessageService,
} from 'packages/bia-ng/core/public-api';
import {
  BiaFrozenColumnDirective,
  BiaTableFilterComponent,
  BiaTableFooterControllerComponent,
  BiaTableInputComponent,
  BiaTableOutputComponent,
  CrudItemTableComponent,
} from 'packages/bia-ng/shared/public-api';
import { PrimeTemplate } from 'primeng/api';
import { Skeleton } from 'primeng/skeleton';
import { TableModule } from 'primeng/table';
import { Tooltip } from 'primeng/tooltip';
import { Pilot } from '../../model/pilot';

@Component({
  selector: 'app-pilot-table',
  templateUrl:
    '../../../../../../packages/bia-ng/shared/components/table/bia-calc-table/bia-calc-table.component.html',
  styleUrls: [
    '../../../../../../packages/bia-ng/shared/components/table/bia-calc-table/bia-calc-table.component.scss',
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
export class PilotTableComponent extends CrudItemTableComponent<Pilot> {
  constructor(
    public formBuilder: UntypedFormBuilder,
    public authService: AuthService,
    public biaMessageService: BiaMessageService,
    public translateService: TranslateService
  ) {
    super(formBuilder, authService, biaMessageService, translateService);
  }

  public addFooterEmptyObject() {
    if (this.canAdd === true) {
      this.footerRowData = { id: '' };
    }
  }
}
