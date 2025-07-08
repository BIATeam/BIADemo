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
import { PrimeTemplate } from 'primeng/api';
import { Skeleton } from 'primeng/skeleton';
import { TableModule } from 'primeng/table';
import { Tooltip } from 'primeng/tooltip';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { BiaFrozenColumnDirective } from 'src/app/shared/bia-shared/components/table/bia-frozen-column/bia-frozen-column.directive';
import { BiaTableFilterComponent } from 'src/app/shared/bia-shared/components/table/bia-table-filter/bia-table-filter.component';
import { BiaTableFooterControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-footer-controller/bia-table-footer-controller.component';
import { BiaTableInputComponent } from 'src/app/shared/bia-shared/components/table/bia-table-input/bia-table-input.component';
import { BiaTableOutputComponent } from 'src/app/shared/bia-shared/components/table/bia-table-output/bia-table-output.component';
import { CrudItemTableComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-table/crud-item-table.component';
import { Site } from '../../model/site';

@Component({
  selector: 'app-site-table',
  templateUrl:
    '../../../../shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component.html',
  styleUrls: [
    '../../../../shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component.scss',
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
