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
  BiaCalcTableCellComponent,
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
import { Engine } from 'src/app/features/planes-specific/model/engine';

@Component({
  selector: 'app-engine-specific-table',
  templateUrl:
    '../../../../../../../../packages/bia-ng/shared/components/table/bia-calc-table/bia-calc-table.component.html',
  styleUrls: [
    '../../../../../../../../packages/bia-ng/shared/components/table/bia-calc-table/bia-calc-table.component.scss',
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    TableModule,
    PrimeTemplate,
    Tooltip,
    BiaTableFilterComponent,
    NgClass,
    BiaTableInputComponent,
    NgTemplateOutlet,
    BiaTableOutputComponent,
    Skeleton,
    NgStyle,
    BiaTableFooterControllerComponent,
    AsyncPipe,
    TranslateModule,
    BiaFrozenColumnDirective,
    BiaCalcTableCellComponent,
  ],
})
export class EngineTableComponent extends CrudItemTableComponent<Engine> {
  constructor(
    public formBuilder: UntypedFormBuilder,
    public authService: AuthService,
    public biaMessageService: BiaMessageService,
    public translateService: TranslateService
  ) {
    super(formBuilder, authService, biaMessageService, translateService);
  }
}
