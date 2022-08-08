import { Component, HostBinding, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { getAllAirports, getAirportsTotalCount, getAirportLoadingGetAll } from '../../store/airport.state';
import { multiRemove, loadAllByPost, update, create } from '../../store/airports-actions';
import { Observable, Subscription } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';
import { Airport } from '../../model/airport';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import {
  BiaListConfig,
  PrimeTableColumn,
} from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';
import { AppState } from 'src/app/store/state';
import { DEFAULT_PAGE_SIZE } from 'src/app/shared/constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AirportDas } from '../../services/airport-das.service';
import * as FileSaver from 'file-saver';
import { TranslateService } from '@ngx-translate/core';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { Permission } from 'src/app/shared/permission';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';
import { AirportsSignalRService } from '../../services/airport-signalr.service';
import { AirportsEffects } from '../../store/airports-effects';
import { loadAllView } from 'src/app/shared/bia-shared/features/view/store/views-actions';

@Component({
  selector: 'app-airports-index',
  templateUrl: './airports-index.component.html',
  styleUrls: ['./airports-index.component.scss']
})
export class AirportsIndexComponent implements OnInit, OnDestroy {
  useCalcMode = false;
  useSignalR = true;
  useView = false;

  private sub = new Subscription();
  @HostBinding('class.bia-flex') flex = true;
  @ViewChild(BiaTableComponent, { static: false }) airportListComponent: BiaTableComponent;
  showColSearch = false;
  globalSearchValue = '';
  defaultPageSize = DEFAULT_PAGE_SIZE;
  pageSize = this.defaultPageSize;
  totalRecords: number;
  airports$: Observable<Airport[]>;
  selectedAirports: Airport[];
  totalCount$: Observable<number>;
  loading$: Observable<boolean>;
  canEdit = false;
  canDelete = false;
  canAdd = false;
  tableConfiguration: BiaListConfig;
  columns: KeyValuePair[];
  displayedColumns: KeyValuePair[];
  viewPreference: string;
  popupTitle: string;
  tableStateKey = this.useView ? 'airportsGrid' : undefined;


  constructor(
    private store: Store<AppState>,
    private router: Router,
    public activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private airportDas: AirportDas,
    private translateService: TranslateService,
    private biaTranslationService: BiaTranslationService,
    private airportsSignalRService: AirportsSignalRService,
  ) {
  }

  ngOnInit() {
    this.initTableConfiguration();
    this.setPermissions();
    this.airports$ = this.store.select(getAllAirports);
    this.totalCount$ = this.store.select(getAirportsTotalCount);
    this.loading$ = this.store.select(getAirportLoadingGetAll);
    this.OnDisplay();
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
      this.airportsSignalRService.initialize();
      AirportsEffects.useSignalR = true;
    }
  }

  OnHide() {
    if (this.useSignalR) {
      AirportsEffects.useSignalR = false;
      this.airportsSignalRService.destroy();
    }
  }

  onCreate() {
    if (!this.useCalcMode) {
      this.router.navigate(['../create'], { relativeTo: this.activatedRoute });
    }
  }

  onEdit(airportId: number) {
    if (!this.useCalcMode) {
      this.router.navigate(['../' + airportId + '/edit'], { relativeTo: this.activatedRoute });
    }
  }

  onSave(airport: Airport) {
    if (this.useCalcMode) {
      if (airport?.id > 0) {
        if (this.canEdit) {
          this.store.dispatch(update({ airport: airport }));
        }
      } else {
        if (this.canAdd) {
          this.store.dispatch(create({ airport: airport }));
        }
      }
    }
  }

  onDelete() {
    if (this.selectedAirports && this.canDelete) {
      this.store.dispatch(multiRemove({ ids: this.selectedAirports.map((x) => x.id) }));
    }
  }

  onSelectedElementsChanged(airports: Airport[]) {
    this.selectedAirports = airports;
  }

  onPageSizeChange(pageSize: number) {
    this.pageSize = pageSize;
  }

  onLoadLazy(lazyLoadEvent: LazyLoadEvent) {
    this.store.dispatch(loadAllByPost({ event: lazyLoadEvent }));
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
    this.airportListComponent.getPrimeNgTable().columns.map((x: PrimeTableColumn) => (columns[x.field] = this.translateService.instant(x.header)));
    const customEvent: any = { columns: columns, ...this.airportListComponent.getLazyLoadMetadata() };
    this.airportDas.getFile(customEvent).subscribe((data) => {
      FileSaver.saveAs(data, this.translateService.instant('app.airports') + '.csv');
    });
  }

  private setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Airport_Update);
    this.canDelete = this.authService.hasPermission(Permission.Airport_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Airport_Create);
  }

  private initTableConfiguration() {
    this.sub.add(this.biaTranslationService.currentCultureDateFormat$.subscribe((dateFormat) => {
      this.tableConfiguration = {
        columns: [
          new PrimeTableColumn('name', 'airport.name'),
          new PrimeTableColumn('city', 'airport.city')
        ]
      };

      this.columns = this.tableConfiguration.columns.map((col) => <KeyValuePair>{ key: col.field, value: col.header });
      this.displayedColumns = [...this.columns];
    }));
  }
}
