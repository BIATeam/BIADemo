import { Component, HostBinding, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { getAllAirports, getAirportsTotalCount, getAirportLoadingGetAll } from '../../store/airport.state';
import { multiRemove, loadAllByPost, load, openDialogEdit, openDialogNew } from '../../store/airports-actions';
import { Observable } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';
import { Airport } from '../../model/airport';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import { BiaListConfig, PrimeTableColumn } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';
import { AppState } from 'src/app/store/state';
import { DEFAULT_PAGE_SIZE } from 'src/app/shared/constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { AirportDas } from '../../services/airport-das.service';
import * as FileSaver from 'file-saver';
import { TranslateService } from '@ngx-translate/core';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { Permission } from 'src/app/shared/permission';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';

@Component({
  selector: 'app-airports-index',
  templateUrl: './airports-index.component.html',
  styleUrls: ['./airports-index.component.scss']
})
export class AirportsIndexComponent implements OnInit {
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

  constructor(
    private store: Store<AppState>,
    private authService: AuthService,
    private airportDas: AirportDas,
    private translateService: TranslateService,
    private biaTranslationService: BiaTranslationService
  ) {}

  ngOnInit() {
    this.initTableConfiguration();
    this.setPermissions();
    this.airports$ = this.store.select(getAllAirports).pipe();
    this.totalCount$ = this.store.select(getAirportsTotalCount).pipe();
    this.loading$ = this.store.select(getAirportLoadingGetAll).pipe();
  }

  onCreate() {
    this.store.dispatch(openDialogNew());
  }

  onEdit(airportId: number) {
    this.store.dispatch(load({ id: airportId }));
    this.store.dispatch(openDialogEdit());
  }

  onDelete() {
    if (this.selectedAirports) {
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
    this.columns.map((x) => (columns[x.value.split('.')[1]] = this.translateService.instant(x.value)));
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
    this.biaTranslationService.culture$.subscribe((dateFormat) => {
      this.tableConfiguration = {
        columns: [new PrimeTableColumn('name', 'airport.name'), new PrimeTableColumn('city', 'airport.city')]
      };

      this.columns = this.tableConfiguration.columns.map((col) => <KeyValuePair>{ key: col.field, value: col.header });
      this.displayedColumns = [...this.columns];
    });
  }
}
