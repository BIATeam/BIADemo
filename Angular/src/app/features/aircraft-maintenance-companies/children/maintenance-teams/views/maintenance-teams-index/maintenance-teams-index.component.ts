import { Component, HostBinding, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { getAllMaintenanceTeams, getMaintenanceTeamsTotalCount, getMaintenanceTeamLoadingGetAll } from '../../store/maintenance-team.state';
import { FeatureMaintenanceTeamsActions } from '../../store/maintenance-teams-actions';
import { Observable, Subscription } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';
import { MaintenanceTeam } from '../../model/maintenance-team';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import {
  BiaListConfig,
  PrimeTableColumn,
} from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';
import { AppState } from 'src/app/store/state';
import { DEFAULT_PAGE_SIZE, TeamTypeId } from 'src/app/shared/constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MaintenanceTeamDas } from '../../services/maintenance-team-das.service';
import * as FileSaver from 'file-saver';
import { TranslateService } from '@ngx-translate/core';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { Permission } from 'src/app/shared/permission';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';
import { MaintenanceTeamsSignalRService } from '../../services/maintenance-team-signalr.service';
import { loadAllView } from 'src/app/shared/bia-shared/features/view/store/views-actions';
import { MaintenanceTeamOptionsService } from '../../services/maintenance-team-options.service';
import { PagingFilterFormatDto } from 'src/app/shared/bia-shared/model/paging-filter-format';
import { useCalcMode, useSignalR, useView, useViewTeamWithTypeId } from '../../maintenance-team.constants';
import { AircraftMaintenanceCompanyService } from 'src/app/features/aircraft-maintenance-companies/services/aircraft-maintenance-company.service';
import { MaintenanceTeamTableComponent } from '../../components/maintenance-team-table/maintenance-team-table.component';
import { getAllTeamsOfType } from 'src/app/domains/team/store/team.state';

@Component({
  selector: 'app-maintenance-teams-index',
  templateUrl: './maintenance-teams-index.component.html',
  styleUrls: ['./maintenance-teams-index.component.scss']
})
export class MaintenanceTeamsIndexComponent implements OnInit, OnDestroy {
  useCalcMode = useCalcMode;
  useSignalR = useSignalR;
  useView = useView;
  useRefreshAtLanguageChange = false;
  tableStateKey = this.useView ? 'maintenance-teamsGrid' : undefined;
  useViewTeamWithTypeId = this.useView ? useViewTeamWithTypeId : null;

  @HostBinding('class.bia-flex') flex = true;
  @ViewChild(BiaTableComponent, { static: false }) biaTableComponent: BiaTableComponent;
  @ViewChild(MaintenanceTeamTableComponent, { static: false }) maintenanceTeamTableComponent: MaintenanceTeamTableComponent;
  private get maintenanceTeamListComponent() {
    if (this.biaTableComponent !== undefined) {
      return this.biaTableComponent;
    }
    return this.maintenanceTeamTableComponent;
  }

  private sub = new Subscription();
  showColSearch = false;
  globalSearchValue = '';
  defaultPageSize = DEFAULT_PAGE_SIZE;
  pageSize = this.defaultPageSize;
  totalRecords: number;
  maintenanceTeams$: Observable<MaintenanceTeam[]>;
  selectedMaintenanceTeams: MaintenanceTeam[];
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
  parentIds: string[];

  constructor(
    private store: Store<AppState>,
    private router: Router,
    public activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private maintenanceTeamDas: MaintenanceTeamDas,
    private translateService: TranslateService,
    private biaTranslationService: BiaTranslationService,
    private maintenanceTeamsSignalRService: MaintenanceTeamsSignalRService,
    public maintenanceTeamOptionsService: MaintenanceTeamOptionsService,
    public aircraftMaintenanceCompanyService: AircraftMaintenanceCompanyService,
  ) {
  }

  ngOnInit() {
    this.parentIds = ['' + this.aircraftMaintenanceCompanyService.currentAircraftMaintenanceCompanyId];
    this.sub = new Subscription();

    this.initTableConfiguration();
    this.sub.add(
      this.store.select(getAllTeamsOfType(TeamTypeId.MaintenanceTeam)).subscribe(() => {
        this.setPermissions();
      })
    );
    this.setPermissions();
    this.maintenanceTeams$ = this.store.select(getAllMaintenanceTeams);
    this.totalCount$ = this.store.select(getMaintenanceTeamsTotalCount);
    this.loading$ = this.store.select(getMaintenanceTeamLoadingGetAll);
    this.OnDisplay();
    if (this.useCalcMode) {
      this.sub.add(
        this.biaTranslationService.currentCulture$.subscribe(event => {
            this.maintenanceTeamOptionsService.loadAllOptions();
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
              this.onLoadLazy(this.maintenanceTeamListComponent.getLazyLoadMetadata());
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
      this.maintenanceTeamsSignalRService.initialize();
    }
  }

  OnHide() {
    if (this.useSignalR) {
      this.maintenanceTeamsSignalRService.destroy();
    }
  }

  onCreate() {
    if (!this.useCalcMode) {
      this.router.navigate(['../create'], { relativeTo: this.activatedRoute });
    }
  }

  onEdit() {
    if (this.selectedMaintenanceTeams.length == 1) {
      this.router.navigate(['../' + this.selectedMaintenanceTeams[0].id + '/edit'], { relativeTo: this.activatedRoute });
    }
  }

  
  onManageMember(maintenanceTeamId: number) {
    if (maintenanceTeamId && maintenanceTeamId > 0) {
      this.router.navigate(['../' + maintenanceTeamId + '/members'], { relativeTo: this.activatedRoute });
    }
  }

  onSave(maintenanceTeam: MaintenanceTeam) {
    if (this.useCalcMode) {
      if (maintenanceTeam?.id > 0) {
        if (this.canEdit) {
          this.store.dispatch(FeatureMaintenanceTeamsActions.update({ maintenanceTeam: maintenanceTeam }));
        }
      } else {
        if (this.canAdd) {
          this.store.dispatch(FeatureMaintenanceTeamsActions.create({ maintenanceTeam: maintenanceTeam }));
        }
      }
    }
  }

  onDelete() {
    if (this.selectedMaintenanceTeams && this.canDelete) {
      this.store.dispatch(FeatureMaintenanceTeamsActions.multiRemove({ ids: this.selectedMaintenanceTeams.map((x) => x.id) }));
    }
  }

  onSelectedElementsChanged(maintenanceTeams: MaintenanceTeam[]) {
    this.selectedMaintenanceTeams = maintenanceTeams;
  }

  onPageSizeChange(pageSize: number) {
    this.pageSize = pageSize;
  }

  onLoadLazy(lazyLoadEvent: LazyLoadEvent) {
    const pagingAndFilter: PagingFilterFormatDto = { parentIds: this.parentIds, ...lazyLoadEvent };
    this.store.dispatch(FeatureMaintenanceTeamsActions.loadAllByPost({ event: pagingAndFilter }));
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
    this.columns.map((x) => (columns[x.value.split('.')[1]] = this.translateService.instant(x.value)));
    const columnsAndFilter: PagingFilterFormatDto = {
      parentIds: this.parentIds, columns: columns, ...this.maintenanceTeamListComponent.getLazyLoadMetadata()
    };
    this.maintenanceTeamDas.getFile(columnsAndFilter).subscribe((data) => {
      FileSaver.saveAs(data, this.translateService.instant('app.maintenanceTeams') + '.csv');
    });
  }

  private setPermissions() {
    this.canDetail = this.authService.hasPermission(Permission.MaintenanceTeam_Member_List_Access);
    this.canEdit = this.authService.hasPermission(Permission.MaintenanceTeam_Update);
    this.canDelete = this.authService.hasPermission(Permission.MaintenanceTeam_Delete);
    this.canAdd = this.authService.hasPermission(Permission.MaintenanceTeam_Create);
  }

  private initTableConfiguration() {
    this.biaTranslationService.currentCultureDateFormat$.subscribe((dateFormat) => {
      this.tableConfiguration = {
        columns: [
          new PrimeTableColumn('title', 'maintenanceTeam.title'),
        ]
      };

      this.columns = this.tableConfiguration.columns.map((col) => <KeyValuePair>{ key: col.field, value: col.header });
      this.displayedColumns = [...this.columns];
    });
  }
}
