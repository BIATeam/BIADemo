import { BiaTableFilterComponent } from 'src/app/shared/bia-shared/components/table/bia-table-filter/bia-table-filter.component';
import { BiaTableInputComponent } from 'src/app/shared/bia-shared/components/table/bia-table-input/bia-table-input.component';
import { BiaTableOutputComponent } from 'src/app/shared/bia-shared/components/table/bia-table-output/bia-table-output.component';
import { BiaTableFooterControllerComponent } from 'src/app/shared/bia-shared/components/table/bia-table-footer-controller/bia-table-footer-controller.component';
import { Component } from '@angular/core';
import {
  UntypedFormBuilder,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { TranslateService, TranslateModule } from '@ngx-translate/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { CrudItemTableComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-table/crud-item-table.component';
import { Plane } from '../../model/plane';
import {
  NgIf,
  NgFor,
  NgSwitch,
  NgClass,
  NgTemplateOutlet,
  NgStyle,
  AsyncPipe,
} from '@angular/common';
import { TableModule } from 'primeng/table';
import { PrimeTemplate } from 'primeng/api';
import { Tooltip } from 'primeng/tooltip';
import { BiaSharedModule } from '../../../../shared/bia-shared/bia-shared.module';
import { Skeleton } from 'primeng/skeleton';

@Component({
  selector: 'app-plane-specific-table',
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
    BiaSharedModule,
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
  ],
})
export class PlaneTableComponent extends CrudItemTableComponent<Plane> {
  constructor(
    public formBuilder: UntypedFormBuilder,
    public authService: AuthService,
    public biaMessageService: BiaMessageService,
    public translateService: TranslateService
  ) {
    super(formBuilder, authService, biaMessageService, translateService);
  }
}
