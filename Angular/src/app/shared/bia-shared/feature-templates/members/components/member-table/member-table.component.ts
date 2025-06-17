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
import { CrudItemTableComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-table/crud-item-table.component';
import { BiaTableFilterComponent } from '../../../../components/table/bia-table-filter/bia-table-filter.component';
import { BiaTableFooterControllerComponent } from '../../../../components/table/bia-table-footer-controller/bia-table-footer-controller.component';
import { BiaTableInputComponent } from '../../../../components/table/bia-table-input/bia-table-input.component';
import { BiaTableOutputComponent } from '../../../../components/table/bia-table-output/bia-table-output.component';
import { Member } from '../../model/member';

@Component({
  selector: 'bia-member-table',
  templateUrl:
    '../../../../components/table/bia-calc-table/bia-calc-table.component.html',
  styleUrls: [
    '../../../../components/table/bia-calc-table/bia-calc-table.component.scss',
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
  ],
})
export class MemberTableComponent extends CrudItemTableComponent<Member> {
  constructor(
    public formBuilder: UntypedFormBuilder,
    public authService: AuthService,
    public biaMessageService: BiaMessageService,
    public translateService: TranslateService
  ) {
    super(formBuilder, authService, biaMessageService, translateService);
  }
}
