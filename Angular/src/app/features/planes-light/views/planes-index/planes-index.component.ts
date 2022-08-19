import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { Plane } from '../../model/plane';
import {
  PrimeTableColumn,
  PropType,
  PrimeNGFiltering
} from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';
import { AppState } from 'src/app/store/state';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { Permission } from 'src/app/shared/permission';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
import { PlaneFacadeService } from '../../services/plane-facade.service';

@Component({
  selector: 'app-planes-index',
  templateUrl: './planes-index.component.html',
  styleUrls: ['./planes-index.component.scss']
})
export class PlanesIndexComponent extends CrudItemsIndexComponent<Plane> {
  constructor(
    protected store: Store<AppState>,
    protected router: Router,
    public activatedRoute: ActivatedRoute,
    protected authService: AuthService,
    protected translateService: TranslateService,
    protected biaTranslationService: BiaTranslationService,
    protected facadeService: PlaneFacadeService, 
  ) {
    super(store,router,activatedRoute,translateService,
      biaTranslationService,facadeService);
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Plane_Update);
    this.canDelete = this.authService.hasPermission(Permission.Plane_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Plane_Create);
  }

  protected initTableConfiguration() {
    this.sub.add(this.biaTranslationService.currentCultureDateFormat$.subscribe((dateFormat) => {
      this.tableConfiguration = {
        columns: [
          new PrimeTableColumn('msn', 'plane.msn'),
          Object.assign(new PrimeTableColumn('isActive', 'plane.isActive'), {
            isSearchable: false,
            isSortable: false,
            type: PropType.Boolean
          }),
          Object.assign(new PrimeTableColumn('lastFlightDate', 'plane.lastFlightDate'), {
            type: PropType.DateTime,
            formatDate: dateFormat.dateTimeFormat
          }),
          Object.assign(new PrimeTableColumn('deliveryDate', 'plane.deliveryDate'), {
            type: PropType.Date,
            formatDate: dateFormat.dateFormat
          }),
          Object.assign(new PrimeTableColumn('syncTime', 'plane.syncTime'), {
            type: PropType.TimeSecOnly,
            formatDate: dateFormat.timeFormatSec
          }),
          Object.assign(new PrimeTableColumn('capacity', 'plane.capacity'), {
            type: PropType.Number,
            filterMode: PrimeNGFiltering.Equals
          }),
          Object.assign(new PrimeTableColumn('planeType', 'plane.planeType'), {
            type: PropType.OneToMany
          }),
          Object.assign(new PrimeTableColumn('connectingAirports', 'plane.connectingAirports'), {
            type: PropType.ManyToMany
          })
        ]
      };

      this.columns = this.tableConfiguration.columns.map((col) => <KeyValuePair>{ key: col.field, value: col.header });
      this.displayedColumns = [...this.columns];
    }));
  }
}
