import { Component, HostBinding, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { getAllAircraftMaintenanceCompanies, getAircraftMaintenanceCompaniesTotalCount, getAircraftMaintenanceCompanyLoadingGetAll } from '../../store/aircraft-maintenance-company.state';
import { FeatureAircraftMaintenanceCompaniesActions } from '../../store/aircraft-maintenance-companies-actions';
import { Observable, Subscription } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';
import { AircraftMaintenanceCompany } from '../../model/aircraft-maintenance-company';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import {
  BiaListConfig,
  PrimeTableColumn,
} from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';
import { AppState } from 'src/app/store/state';
import { DEFAULT_PAGE_SIZE, TeamTypeId } from 'src/app/shared/constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AircraftMaintenanceCompanyDas } from '../../services/aircraft-maintenance-company-das.service';
import * as FileSaver from 'file-saver';
import { TranslateService } from '@ngx-translate/core';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { Permission } from 'src/app/shared/permission';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';
import { AircraftMaintenanceCompaniesSignalRService } from '../../services/aircraft-maintenance-company-signalr.service';
import { loadAllView } from 'src/app/shared/bia-shared/features/view/store/views-actions';
import { AircraftMaintenanceCompanyOptionsService } from '../../services/aircraft-maintenance-company-options.service';
import { PagingFilterFormatDto } from 'src/app/shared/bia-shared/model/paging-filter-format';
import { AircraftMaintenanceCompanyTableComponent } from 'src/app/features/aircraft-maintenance-companies/components/aircraft-maintenance-company-table/aircraft-maintenance-company-table.component';
import { useCalcMode, useSignalR, useView } from '../../aircraft-maintenance-company.constants';
import { getAllTeamsOfType } from 'src/app/domains/bia-domains/team/store/team.state';

@Component({
  selector: 'app-aircraft-maintenance-companies-index',
  templateUrl: './aircraft-maintenance-companies-index.component.html',
  styleUrls: ['./aircraft-maintenance-companies-index.component.scss']
})
export class AircraftMaintenanceCompaniesIndexComponent implements OnInit, OnDestroy {
  useCalcMode = useCalcMode;
  useSignalR = useSignalR;
  useView = useView;
  useRefreshAtLanguageChange = false;

  @HostBinding('class.bia-flex') flex = true;
  @ViewChild(BiaTableComponent, { static: false }) biaTableComponent: BiaTableComponent;
  @ViewChild(AircraftMaintenanceCompanyTableComponent, { static: false }) aircraftMaintenanceCompanyTableComponent: AircraftMaintenanceCompanyTableComponent;
  private get aircraftMaintenanceCompanyListComponent() {
    if (this.biaTableComponent !== undefined) {
      return this.biaTableComponent;
    }
    return this.aircraftMaintenanceCompanyTableComponent;
  }

  private sub = new Subscription();
  showColSearch = false;
  globalSearchValue = '';
  defaultPageSize = DEFAULT_PAGE_SIZE;
  pageSize = this.defaultPageSize;
  totalRecords: number;
  aircraftMaintenanceCompanies$: Observable<AircraftMaintenanceCompany[]>;
  selectedAircraftMaintenanceCompanies: AircraftMaintenanceCompany[];
  totalCount$: Observable<number>;
  loading$: Observable<boolean>;
  canEdit = false;
  canDetail = false;
  canDelete = false;
  canAdd = false;
  tableConfiguration: BiaListConfig;
  columns: KeyValuePair[];
  displayedColumns: KeyValuePair[];
  viewPreference: string;
  popupTitle: string;
  tableStateKey = this.useView ? 'aircraft-maintenance-companiesGrid' : undefined;
  parentIds: string[];

  constructor(
    private store: Store<AppState>,
    private router: Router,
    public activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private aircraftMaintenanceCompanyDas: AircraftMaintenanceCompanyDas,
    private translateService: TranslateService,
    private biaTranslationService: BiaTranslationService,
    private aircraftMaintenanceCompaniesSignalRService: AircraftMaintenanceCompaniesSignalRService,
    public aircraftMaintenanceCompanyOptionsService: AircraftMaintenanceCompanyOptionsService,
  ) {
  }

  ngOnInit() {
    this.parentIds = [];
    this.sub = new Subscription();

    this.initTableConfiguration();
    this.sub.add(
      this.store.select(getAllTeamsOfType(TeamTypeId.AircraftMaintenanceCompany)).subscribe(() => {
        this.setPermissions();
      })
    );
    this.aircraftMaintenanceCompanies$ = this.store.select(getAllAircraftMaintenanceCompanies);
    this.totalCount$ = this.store.select(getAircraftMaintenanceCompaniesTotalCount);
    this.loading$ = this.store.select(getAircraftMaintenanceCompanyLoadingGetAll);
    this.OnDisplay();
    if (this.useCalcMode) {
      this.sub.add(
        this.biaTranslationService.currentCulture$.subscribe(event => {
          this.aircraftMaintenanceCompanyOptionsService.loadAllOptions();
        })
      );
    }
    if (this.useRefreshAtLanguageChange) {
      // Reload data if language change.
      let isinit = true;
      this.sub.add(
        this.biaTranslationService.currentCulture$.subscribe(event => {
          if (isinit) {
            isinit = false;
          } else {
            this.onLoadLazy(this.aircraftMaintenanceCompanyListComponent.getLazyLoadMetadata());
          }
        })
      );
    }
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
    this.OnHide();
  }

  OnDisplay() {
    if (this.useView) {
      this.store.dispatch(loadAllView());
    }


    if (this.useSignalR) {
      this.aircraftMaintenanceCompaniesSignalRService.initialize();
    }
  }

  OnHide() {
    if (this.useSignalR) {
      this.aircraftMaintenanceCompaniesSignalRService.destroy();
    }
  }

  onCreate() {
    if (!this.useCalcMode) {
      this.router.navigate(['create'], { relativeTo: this.activatedRoute });
    }
  }

  onEdit() {
    if (this.selectedAircraftMaintenanceCompanies.length == 1) {
      this.router.navigate([this.selectedAircraftMaintenanceCompanies[0].id, 'edit'], { relativeTo: this.activatedRoute });
    }
  }

  onMaintenanceTeams() {
    if (this.selectedAircraftMaintenanceCompanies.length == 1) {
      this.router.navigate([this.selectedAircraftMaintenanceCompanies[0].id, 'maintenance-teams'], { relativeTo: this.activatedRoute });
    }
  }

  onManageMember(aircraftMaintenanceCompanyId: number) {
    if (aircraftMaintenanceCompanyId && aircraftMaintenanceCompanyId > 0) {
      this.router.navigate([aircraftMaintenanceCompanyId, 'members'], { relativeTo: this.activatedRoute });
    }
  }

  onSave(aircraftMaintenanceCompany: AircraftMaintenanceCompany) {
    if (this.useCalcMode) {
      if (aircraftMaintenanceCompany?.id > 0) {
        if (this.canEdit) {
          this.store.dispatch(FeatureAircraftMaintenanceCompaniesActions.update({ aircraftMaintenanceCompany: aircraftMaintenanceCompany }));
        }
      } else {
        if (this.canAdd) {
          this.store.dispatch(FeatureAircraftMaintenanceCompaniesActions.create({ aircraftMaintenanceCompany: aircraftMaintenanceCompany }));
        }
      }
    }
  }

  onDelete() {
    if (this.selectedAircraftMaintenanceCompanies && this.canDelete) {
      this.store.dispatch(FeatureAircraftMaintenanceCompaniesActions.multiRemove({ ids: this.selectedAircraftMaintenanceCompanies.map((x) => x.id) }));
    }
  }

  onSelectedElementsChanged(aircraftMaintenanceCompanies: AircraftMaintenanceCompany[]) {
    this.selectedAircraftMaintenanceCompanies = aircraftMaintenanceCompanies;
  }

  onPageSizeChange(pageSize: number) {
    this.pageSize = pageSize;
  }

  onLoadLazy(lazyLoadEvent: LazyLoadEvent) {
    const pagingAndFilter: PagingFilterFormatDto = { parentIds: this.parentIds, ...lazyLoadEvent };
    this.store.dispatch(FeatureAircraftMaintenanceCompaniesActions.loadAllByPost({ event: pagingAndFilter }));
  }

  searchGlobalChanged(value: string) {
    this.globalSearchValue = value;
  }

  displayedColumnsChanged(values: KeyValuePair[]) {
    this.displayedColumns = values;
  }

  onToggleSearch() {
    this.showColSearch = !this.showColSearch;
  }

  onViewChange(viewPreference: string) {
    this.viewPreference = viewPreference;
  }

  onExportCSV() {
    const columns: { [key: string]: string } = {};
    this.aircraftMaintenanceCompanyListComponent.getPrimeNgTable().columns.map((x: PrimeTableColumn) => (columns[x.header.split('.')[1]] = this.translateService.instant(x.header)));
    const columnsAndFilter: PagingFilterFormatDto = {
      parentIds: this.parentIds, columns: columns, ...this.aircraftMaintenanceCompanyListComponent.getLazyLoadMetadata()
    };
    this.aircraftMaintenanceCompanyDas.getFile(columnsAndFilter).subscribe((data) => {
      FileSaver.saveAs(data, this.translateService.instant('app.aircraftMaintenanceCompanies') + '.csv');
    });
  }

  private setPermissions() {
    this.canDetail = this.authService.hasPermission(Permission.AircraftMaintenanceCompany_Member_List_Access);
    this.canEdit = this.authService.hasPermission(Permission.AircraftMaintenanceCompany_Update);
    this.canDelete = this.authService.hasPermission(Permission.AircraftMaintenanceCompany_Delete);
    this.canAdd = this.authService.hasPermission(Permission.AircraftMaintenanceCompany_Create);
  }

  private initTableConfiguration() {
    this.biaTranslationService.currentCultureDateFormat$.subscribe((dateFormat) => {
      this.tableConfiguration = {
        columns: [
          new PrimeTableColumn('title', 'aircraftMaintenanceCompany.title'),
        ]
      };

      this.columns = this.tableConfiguration.columns.map((col) => <KeyValuePair>{ key: col.field, value: col.header });
      this.displayedColumns = [...this.columns];
    });
  }
}
