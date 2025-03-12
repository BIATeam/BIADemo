import { Component } from '@angular/core';
import { UntypedFormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateService, TranslateModule } from '@ngx-translate/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { CrudItemTableComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-table/crud-item-table.component';
import { Airport } from '../../model/airport';
import { NgIf, NgFor, NgSwitch, NgClass, NgTemplateOutlet, NgStyle, AsyncPipe } from '@angular/common';
import { TableModule } from 'primeng/table';
import { PrimeTemplate } from 'primeng/api';
import { Tooltip } from 'primeng/tooltip';
import { BiaTableFilterComponent } from '../../../../shared/bia-shared/components/table/bia-table-filter/bia-table-filter.component';
import { BiaTableInputComponent } from '../../../../shared/bia-shared/components/table/bia-table-input/bia-table-input.component';
import { BiaTableOutputComponent } from '../../../../shared/bia-shared/components/table/bia-table-output/bia-table-output.component';
import { Skeleton } from 'primeng/skeleton';
import { BiaTableFooterControllerComponent } from '../../../../shared/bia-shared/components/table/bia-table-footer-controller/bia-table-footer-controller.component';

@Component({
    selector: 'app-airport-table',
    templateUrl: '../../../../shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component.html',
    styleUrls: [
        '../../../../shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component.scss',
    ],
    imports: [FormsModule, ReactiveFormsModule, NgIf, TableModule, PrimeTemplate, NgFor, Tooltip, NgSwitch, BiaTableFilterComponent, NgClass, BiaTableInputComponent, NgTemplateOutlet, BiaTableOutputComponent, Skeleton, NgStyle, BiaTableFooterControllerComponent, AsyncPipe, TranslateModule]
})
export class AirportTableComponent extends CrudItemTableComponent<Airport> {
  constructor(
    public formBuilder: UntypedFormBuilder,
    public authService: AuthService,
    public biaMessageService: BiaMessageService,
    public translateService: TranslateService
  ) {
    super(formBuilder, authService, biaMessageService, translateService);
  }
}
