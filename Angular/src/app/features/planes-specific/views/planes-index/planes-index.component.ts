import { AsyncPipe, CommonModule, NgClass } from '@angular/common';
import { Component, Injector, ViewChild } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import {
  AuthService,
  BiaSignalRService,
} from 'packages/bia-ng/core/public-api';
import {
  BiaFieldsConfig,
  DataResult,
  KeyValuePair,
  PagingFilterFormatDto,
} from 'packages/bia-ng/models/public-api';
import {
  BiaTableBehaviorControllerComponent,
  BiaTableComponent,
  BiaTableControllerComponent,
  BiaTableHeaderComponent,
  CrudItemService,
  CrudItemsIndexComponent,
  SpinnerComponent,
} from 'packages/bia-ng/shared/public-api';
import { PrimeTemplate } from 'primeng/api';
import { TableModule, TableRowExpandEvent } from 'primeng/table';
import { map, take } from 'rxjs';
import { EngineDas } from 'src/app/features/planes/children/engines/services/engine-das.service';
import { Permission } from 'src/app/shared/permission';
import { engineCRUDConfiguration } from '../../children/engines/engine.constants';
import { PlaneTableComponent } from '../../components/plane-table/plane-table.component';
import { Engine } from '../../model/engine';
import { Plane } from '../../model/plane';
import { PlaneSpecific } from '../../model/plane-specific';
import { planeCRUDConfiguration } from '../../plane.constants';
import { PlaneService } from '../../services/plane.service';

@Component({
  selector: 'app-planes-specific-index',
  templateUrl: './planes-index.component.html',
  styleUrls: ['./planes-index.component.scss'],
  imports: [
    NgClass,
    PrimeTemplate,
    AsyncPipe,
    TranslateModule,
    BiaTableHeaderComponent,
    BiaTableControllerComponent,
    BiaTableBehaviorControllerComponent,
    BiaTableComponent,
    CommonModule,
    TableModule,
    SpinnerComponent,
  ],
  providers: [{ provide: CrudItemService, useExisting: PlaneService }],
})
export class PlanesIndexComponent extends CrudItemsIndexComponent<
  Plane,
  PlaneSpecific
> {
  @ViewChild(PlaneTableComponent, { static: false })
  crudItemTableComponent: PlaneTableComponent;
  enginesDic: { [key: string]: Engine[] } = {};
  engineConfig: BiaFieldsConfig<Engine> = engineCRUDConfiguration.fieldsConfig;
  displayedEngineColumns: KeyValuePair[];
  biaSignalRService: BiaSignalRService;

  constructor(
    protected injector: Injector,
    public planeService: PlaneService,
    protected authService: AuthService,
    protected engineDasService: EngineDas
  ) {
    super(injector, planeService);
    this.crudConfiguration = planeCRUDConfiguration;
    this.displayedEngineColumns = this.engineConfig.columns
      .filter(col => col.isVisibleInTable && !col.isHideByDefault)
      .map(col => <KeyValuePair>{ key: col.field, value: col.header });
    this.biaSignalRService =
      this.injector.get<BiaSignalRService>(BiaSignalRService);
    this.registerSignalRMethods();
  }

  protected registerSignalRMethods() {
    this.biaSignalRService.addMethod(
      this.planeService.getSignalRRefreshEvent() + '-plane-engines',
      (args: any) => {
        const data: { id: number; engines: Engine[] } = JSON.parse(args);
        this.enginesDic[data.id.toString()] = data.engines;
      }
    );
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Plane_Update);
    this.canDelete = this.authService.hasPermission(Permission.Plane_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Plane_Create);
  }

  onRowExpand(event: TableRowExpandEvent) {
    this.getPlaneEngines(event.data.id);
  }

  private getPlaneEngines(planeId: number) {
    const pagingAndFilter: PagingFilterFormatDto = {
      filters: {},
      first: 0,
      globalFilter: null,
      multiSortMeta: [{ field: 'reference', order: 1 }],
      rows: null,
      sortField: 'reference',
      sortOrder: 1,
      parentIds: [planeId.toString()],
    };
    this.engineDasService
      .getListByPost({ event: pagingAndFilter })
      .pipe(
        take(1),
        map((result: DataResult<Engine[]>) => {
          this.enginesDic[planeId.toString()] = result.data;
        })
      )
      .subscribe();
  }
}
